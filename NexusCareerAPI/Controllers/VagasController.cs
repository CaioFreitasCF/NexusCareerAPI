using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusCareerAPI.Data;
using NexusCareerAPI.Models;
using NexusCareerAPI.DTO;

namespace NexusCareerAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class VagasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public VagasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/vagas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Vagas>>> GetVagas()
        {
            return Ok(await _context.Vaga.ToListAsync());
        }

        // GET: api/v1/vagas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Vagas>> GetVaga(int id)
        {
            var vaga = await _context.Vaga.FindAsync(id);

            if (vaga == null)
                return NotFound();

            return Ok(vaga);
        }

        // POST: api/v1/vagas (SEM ID NO BODY)
        [HttpPost]
        public async Task<ActionResult<Vagas>> PostVaga(VagaCreateDto dto)
        {
            var vaga = new Vagas
            {
                Titulo = dto.Titulo,
                Descricao = dto.Descricao,
                Requisitos = dto.Requisitos,
                Empresa = dto.Empresa
            };

            _context.Vaga.Add(vaga);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetVaga), new { id = vaga.Id }, vaga);
        }

        // PUT: api/v1/vagas/5 (SEM ID NO BODY)
        [HttpPut("{id}")]
        public async Task<IActionResult> PutVaga(int id, VagaCreateDto dto)
        {
            var vagaExistente = await _context.Vaga.FindAsync(id);

            if (vagaExistente == null)
                return NotFound("Vaga não encontrada.");

            vagaExistente.Titulo = dto.Titulo;
            vagaExistente.Descricao = dto.Descricao;
            vagaExistente.Requisitos = dto.Requisitos;
            vagaExistente.Empresa = dto.Empresa;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/vagas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteVaga(int id)
        {
            var vaga = await _context.Vaga.FindAsync(id);

            if (vaga == null)
                return NotFound();

            _context.Vaga.Remove(vaga);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
