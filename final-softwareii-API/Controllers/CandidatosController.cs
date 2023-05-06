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
    public class CandidatosController : ControllerBase
    {
        private readonly FinalSoftwareiiContext _context;

        public CandidatosController(FinalSoftwareiiContext context)
        {
            _context = context;
        }

        // GET: api/Candidatos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Candidato>>> GetCandidatos()
        {
          if (_context.Candidatos == null)
          {
              return NotFound();
          }
            return await _context.Candidatos.ToListAsync();
        }

        // GET: api/Candidatos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Candidato>> GetCandidato(int id)
        {
          if (_context.Candidatos == null)
          {
              return NotFound();
          }
            var candidato = await _context.Candidatos.FindAsync(id);

            if (candidato == null)
            {
                return NotFound();
            }

            return candidato;
        }

        //// PUT: api/Candidatos/5
        //// To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        //[HttpPut("{id}")]
        //public async Task<IActionResult> PutCandidato(int id, Candidato candidato)
        //{
        //    if (id != candidato.Dpi)
        //    {
        //        return BadRequest();
        //    }

        //    _context.Entry(candidato).State = EntityState.Modified;

        //    try
        //    {
        //        await _context.SaveChangesAsync();
        //    }
        //    catch (DbUpdateConcurrencyException)
        //    {
        //        if (!CandidatoExists(id))
        //        {
        //            return NotFound();
        //        }
        //        else
        //        {
        //            throw;
        //        }
        //    }

        //    return NoContent();
        //}

        // POST: api/Candidatos
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPost]
        public async Task<ActionResult<Candidato>> PostCandidato(Candidato candidato, string correo, string contraseña)
        {
            var fase1 = await _context.Fases.FindAsync(1);
            var fase2 = await _context.Fases.FindAsync(2);
            var fase3 = await _context.Fases.FindAsync(3);
            var fase4 = await _context.Fases.FindAsync(4);

            if ((bool)fase1.Estado && !(bool)fase2.Estado && !(bool)fase3.Estado && !(bool)fase4.Estado)
            {
                var usuario = await _context.Usuarios.FindAsync(correo);
                if (usuario.Contraseña == contraseña)
                {
                    if (_context.Candidatos == null)
                    {
                        return Problem("Entity set 'FinalSoftwareiiContext.Candidatos'  is null.");
                    }
                    _context.Candidatos.Add(candidato);
                    await _context.SaveChangesAsync();

                    return CreatedAtAction("GetCandidato", new { id = candidato.Dpi }, candidato);
                }
                return Unauthorized();
            }
            return BadRequest();
        }

        //// DELETE: api/Candidatos/5
        //[HttpDelete("{id}")]
        //public async Task<IActionResult> DeleteCandidato(int id)
        //{
        //    if (_context.Candidatos == null)
        //    {
        //        return NotFound();
        //    }
        //    var candidato = await _context.Candidatos.FindAsync(id);
        //    if (candidato == null)
        //    {
        //        return NotFound();
        //    }

        //    _context.Candidatos.Remove(candidato);
        //    await _context.SaveChangesAsync();

        //    return NoContent();
        //}

        private bool CandidatoExists(int id)
        {
            return (_context.Candidatos?.Any(e => e.Dpi == id)).GetValueOrDefault();
        }
    }
}
