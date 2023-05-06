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
    public class FasesController : ControllerBase
    {
        private readonly FinalSoftwareiiContext _context;

        public FasesController(FinalSoftwareiiContext context)
        {
            _context = context;
        }

        // GET: api/Fases
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Fase>>> GetFases()
        {
          if (_context.Fases == null)
          {
              return NotFound();
          }
            return await _context.Fases.ToListAsync();
        }

        //// GET: api/Fases/5
        //[HttpGet("{id}")]
        //public async Task<ActionResult<Fase>> GetFase(int id)
        //{
        //  if (_context.Fases == null)
        //  {
        //      return NotFound();
        //  }
        //    var fase = await _context.Fases.FindAsync(id);

        //    if (fase == null)
        //    {
        //        return NotFound();
        //    }

        //    return fase;
        //}

        // PUT: api/Fases/5
        // To protect from overposting attacks, see https://go.microsoft.com/fwlink/?linkid=2123754
        [HttpPut("{id}")]
        public async Task<IActionResult> PutFase(int id, Fase fase, string correo, string contraseña)
        {
            var usuario = await _context.Usuarios.FindAsync(correo);
            if (usuario.Contraseña == contraseña)
            {

                if (id != fase.Idfases)
                {
                    return BadRequest();
                }

                _context.Entry(fase).State = EntityState.Modified;

                try
                {
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!FaseExists(id))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }

                return NoContent();
            }
            return Unauthorized();
        }

        private bool FaseExists(int id)
        {
            return (_context.Fases?.Any(e => e.Idfases == id)).GetValueOrDefault();
        }
    }
}
