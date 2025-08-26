using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http.HttpResults;
using apiconsorcio.Data;
using apiconsorcio.Models;

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

        [HttpPost]
        public async Task<JsonResult> Criar(string nome, decimal valor, decimal peso, decimal meta)
        {
            Dados dados = new Dados { Nome = nome, Valor = valor, Peso = peso, Meta = meta };
            _context.Dados.Add(dados);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }


        [HttpGet()]
        public JsonResult GetAllSimulations()
        {

            var resultado = _context.Dados.ToList();

            return new JsonResult(Ok(resultado));

        }

    }
}
