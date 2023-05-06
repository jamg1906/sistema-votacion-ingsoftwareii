using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using final_softwareii_API.Models;

namespace final_softwareii_API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class VotosController : ControllerBase
    {
        private readonly FinalSoftwareiiContext _context;

        public VotosController(FinalSoftwareiiContext context)
        {
            _context = context;
        }

        [HttpPost]
        public async Task<ActionResult<Voto>> PostVoto(int dpiVotante, int? dpiCandidato)
        {
            var fase1 = await _context.Fases.FindAsync(1);
            var fase2 = await _context.Fases.FindAsync(2);
            var fase3 = await _context.Fases.FindAsync(3);
            var fase4 = await _context.Fases.FindAsync(4);


            if ((bool)fase1.Estado && (bool)fase2.Estado && (bool)fase3.Estado && !(bool)fase4.Estado)
            {
                if (VotoExists(dpiVotante))
                {
                    var fraudes = _context.Estadisticas.Find("fraudes");
                    fraudes.Cantidad++;
                    _context.Entry(fraudes).State = EntityState.Modified;
                    await _context.SaveChangesAsync();

                    return BadRequest();
                }
                Voto nuevoVoto = new Voto
                {
                    DpiVotante = dpiVotante,
                    DpiCandidato = dpiCandidato,
                    HoraVoto = DateTime.Now,
                    IpVoto = HttpContext.Connection.RemoteIpAddress.ToString(),
                    
                };

                _context.Votos.Add(nuevoVoto);
                await _context.SaveChangesAsync();

                return Ok();
            }
            return BadRequest();
        }


        private bool VotoExists(int id)
        {
            return (_context.Votos?.Any(e => e.DpiVotante == id)).GetValueOrDefault();
        }
    }
}
