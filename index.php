<?php
// URL da sua API
$apiBase = "https://meu-appconsorcio-bnh3ftgydng4gqac.brazilsouth-01.azurewebsites.net";

// Função auxiliar para fazer requisições
function getJson($url) {
 $ch = curl_init($url);
    curl_setopt($ch, CURLOPT_RETURNTRANSFER, true);
    curl_setopt($ch, CURLOPT_SSL_VERIFYPEER, false); // ⚠️ apenas para teste local
    curl_setopt($ch, CURLOPT_FOLLOWLOCATION, true);
    curl_setopt($ch, CURLOPT_TIMEOUT, 10);
    $res = curl_exec($ch);
    if (curl_errno($ch)) {
        echo "<pre>Erro cURL: " . curl_error($ch) . "</pre>";
    }
    curl_close($ch);
    return $res ? json_decode($res) : null;
}

// Buscar dados da campanha e dos funcionários
$campanhaData = getJson("$apiBase/api/Campanhas");
$funcionariosData = getJson("$apiBase/api/Dados");

$campanhaAtual = $campanhaData->value[0]->vrcampanha ?? 0;
$funcionarios = $funcionariosData->value ?? [];
$totalRealizado = array_sum(array_map(fn($f) => $f->valor ?? 0, $funcionarios));
?>
<!DOCTYPE html>
<html lang="pt-BR">
<head>
<meta charset="UTF-8">
<meta name="viewport" content="width=device-width, initial-scale=1.0">
<script src="https://cdn.jsdelivr.net/npm/chart.js"></script>
<title>Painel de Campanhas & Funcionários (PHP)</title>
<style>
body { font-family: Arial, sans-serif; background: #f0f0f0; padding: 20px; color: #333; }
.container { max-width: 1000px; margin: 0 auto; }
.card { background: #fff; padding: 20px; margin-bottom: 20px; border-radius: 8px; box-shadow: 0 2px 8px rgba(0,0,0,0.1); }
.card h2 { margin-top: 0; }
.table { width: 100%; border-collapse: collapse; margin-top: 10px; }
.table th, .table td { border: 1px solid #ccc; padding: 8px; text-align: left; }
.error { color: red; }
</style>
</head>
<body>
<div class="container">

  <div class="card">
    <h2>Progresso da Campanha</h2>
    <canvas id="graficoFuncionarios" width="800" height="400"></canvas>
  </div>

  <div class="card">
    <h2>Campanha</h2>
    <p>Valor atual: <strong><?php echo $campanhaAtual; ?></strong></p>
    <?php if ($campanhaData === null): ?>
      <p class="error">Erro ao buscar campanha.</p>
    <?php endif; ?>
  </div>

  <div class="card">
    <h2>Funcionários</h2>
    <?php if ($funcionariosData === null): ?>
      <p class="error">Erro ao buscar funcionários.</p>
    <?php else: ?>
      <table class="table">
        <thead>
          <tr><th>ID</th><th>Matrícula</th><th>Nome</th><th>Valor</th><th>Peso</th><th>Meta</th></tr>
        </thead>
        <tbody>
        <?php foreach ($funcionarios as $f): ?>
          <tr>
            <td><?php echo $f->id; ?></td>
            <td><?php echo $f->matricula; ?></td>
            <td><?php echo $f->nome; ?></td>
            <td><?php echo $f->valor; ?></td>
            <td><?php echo $f->peso; ?></td>
            <td><?php echo $f->meta; ?></td>
          </tr>
        <?php endforeach; ?>
        </tbody>
      </table>
    <?php endif; ?>
  </div>

</div>

<script>
// Dados vindos do PHP para o gráfico
const labels = ["Campanha", <?php foreach ($funcionarios as $f) echo '"' . $f->nome . '",'; ?>];
const metas = [<?php echo $campanhaAtual; ?>, <?php foreach ($funcionarios as $f) echo ($f->meta ?? 0) . ','; ?>];
const realizados = [<?php echo $totalRealizado; ?>, <?php foreach ($funcionarios as $f) echo ($f->valor ?? 0) . ','; ?>];

const ctx = document.getElementById('graficoFuncionarios').getContext('2d');
new Chart(ctx, {
  type: 'bar',
  data: {
    labels,
    datasets: [
      { label: 'Meta', data: metas, backgroundColor: 'rgba(0,0,255,1)' },
      { label: 'Realizado', data: realizados, backgroundColor: 'rgba(250,117,4,1)' }
    ]
  },
  options: { responsive:true, plugins:{legend:{position:'top'}, title:{display:true,text:'Meta x Realizado'}, tooltip:{mode:'index',intersect:false}}, scales:{y:{beginAtZero:true}, x:{stacked:false}} }
});
</script>
</body>
</html>
