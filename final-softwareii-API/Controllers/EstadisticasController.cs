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
    public class EstadisticasController : ControllerBase
    {
        private readonly FinalSoftwareiiContext _context;

        public EstadisticasController(FinalSoftwareiiContext context)
        {
            _context = context;
        }

        [Route("/api/Estadisticas/Fraudes")]
        [HttpGet]
        public async Task<ActionResult<Estadistica>> GetFraudes()
        {
            var fase1 = await _context.Fases.FindAsync(1);
            var fase2 = await _context.Fases.FindAsync(2);
            var fase3 = await _context.Fases.FindAsync(3);
            var fase4 = await _context.Fases.FindAsync(4);

            if (((bool)fase1.Estado && (bool)fase2.Estado && (bool)fase3.Estado && !(bool)fase4.Estado) || ((bool)fase1.Estado && (bool)fase2.Estado && (bool)fase3.Estado && (bool)fase4.Estado))
            {

                if (_context.Estadisticas == null)
                {
                    return NotFound();
                }
                var estadistica = await _context.Estadisticas.FindAsync("fraudes");

                if (estadistica == null)
                {
                    return NotFound();
                }

                return estadistica;
            }

            return BadRequest();
        }

        [Route("/api/Estadisticas/VotosActuales")]
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidatura>>> GetVotos()
        {
            var fase1 = await _context.Fases.FindAsync(1);
            var fase2 = await _context.Fases.FindAsync(2);
            var fase3 = await _context.Fases.FindAsync(3);
            var fase4 = await _context.Fases.FindAsync(4);

            if (((bool)fase1.Estado && (bool)fase2.Estado && (bool)fase3.Estado && !(bool)fase4.Estado) || ((bool)fase1.Estado && (bool)fase2.Estado && (bool)fase3.Estado && (bool)fase4.Estado))
            {
                var votos = await _context.Votos.ToListAsync();
                var candidatos = await _context.Candidatos.ToListAsync();
                var candidaturas = new List<Candidatura>();

                foreach (Candidato Elcandidato in candidatos)
                {
                    candidaturas.Add(new Candidatura { candidato = Elcandidato, votos = 0 });
                }

                candidaturas.Add(new Candidatura
                {
                    candidato = new
                    Candidato
                    {
                        Dpi = 0,
                        FechaNacimiento = null,
                        NombreCompleto = "Votos nulos",
                        PartidoPolitico = null
                    },
                    votos = 0
                });

                foreach (Voto unVoto in votos)
                {
                    if (unVoto.DpiCandidato != null)
                    {
                        candidaturas.Find(s => s.candidato.Dpi == unVoto.DpiCandidato).votos++;
                    }
                    else
                    {
                        candidaturas.Find(s => s.candidato.NombreCompleto == "Votos nulos").votos++;
                    }
                }

                return candidaturas;
            }
            return BadRequest();
        }

    }
    public class Candidatura
    {
        public Candidato candidato { get; set; }
        public int votos { get; set; }
    }
}
