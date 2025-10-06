using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using apiconsorcio.Data;
using apiconsorcio.Models;
using Microsoft.EntityFrameworkCore;

namespace apiconsorcio.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class DadosController : ControllerBase
    {
        private readonly DataContext _context;


        public DadosController(DataContext datacontext)
        {
            _context = datacontext;
        }

        [HttpGet()]
        public JsonResult GetAllSimulations()
        {

            var resultado = _context.Dados.ToList();

            return new JsonResult(Ok(resultado));

        }

        [HttpPost]
        public async Task<JsonResult> Criar(string matricula, string nome, decimal valor, decimal peso, decimal meta)
        {
            Dados dados = new Dados { Matricula=matricula, Nome = nome, Valor = valor, Peso = peso, Meta = meta };
            _context.Dados.Add(dados);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpPost("adicionaValor-valor")]

        public JsonResult AdicionaValor(string matricula, decimal valor)
        {
            var Dados_Localizados = _context.Dados
                .FirstOrDefault(d => d.Matricula == matricula);

            Dados_Localizados.Valor += valor;

            // Salva a alteração no banco
            _context.SaveChanges();

            return new JsonResult(Ok(Dados_Localizados));

        }

        [HttpDelete]

        public JsonResult Excluir(string matricula)
        {
            var Dados_Localizados = _context.Dados
                .FirstOrDefault(d => d.Matricula == matricula);

            _context.Dados.Remove(Dados_Localizados);

            // Salva a alteração no banco
            _context.SaveChanges();

            return new JsonResult(Ok("Registro com nome "+ matricula +" Excluído com sucesso!" ));

        }





    }
}
