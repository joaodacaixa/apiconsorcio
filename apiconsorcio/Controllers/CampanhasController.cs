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
    public class CampanhasController : ControllerBase
    {
        private readonly CampanhaContext _context;


        public CampanhasController(CampanhaContext campanhacontext)
        {
            _context = campanhacontext;
        }

        [HttpGet()]
        public JsonResult GetAllSimulations()
        {
            var resultado = _context.Campanha.ToList();

            return new JsonResult(Ok(resultado));
        }

        [HttpPost]
        public async Task<JsonResult> Criar(decimal valor)
        {
            Campanha campanha = new Campanha { vrcampanha = valor };
            _context.Campanha.Add(campanha);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpPost ("Atualiza_campanha")]
        public async Task<JsonResult> AtualizarCampanha(decimal valor)
        {
            var campanha_existente = _context.Campanha
                .FirstOrDefault(d => d.id == 1);
            campanha_existente.vrcampanha = valor;
            _context.Campanha.Update(campanha_existente);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

        [HttpDelete("Deleta_Campanha")]
        public async Task<JsonResult> DeletearCampanha()
        {
            var campanha_existente = _context.Campanha
                .FirstOrDefault(d => d.id ==4);
            
            _context.Campanha.Remove(campanha_existente);
            _context.SaveChanges();
            return new JsonResult(Ok());
        }

    }
}
