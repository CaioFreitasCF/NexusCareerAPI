using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusCareerAPI.Data;
using NexusCareerAPI.Models;
using NexusCareerAPI.DTO;

namespace NexusCareerAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class CurriculosController : ControllerBase
    {
        private readonly AppDbContext _context;

        public CurriculosController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/curriculos
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Curriculo>>> GetCurriculos()
        {
            return Ok(await _context.Curriculos.ToListAsync());
        }

        // GET: api/v1/curriculos/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Curriculo>> GetCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FindAsync(id);

            if (curriculo == null)
                return NotFound();

            return Ok(curriculo);
        }

        
        // GET: api/v1/curriculos/por-nome/{nome}
        [HttpGet("nome/{nome}")]
        public async Task<ActionResult<IEnumerable<Curriculo>>> GetCurriculosPorNome(string nome)
        {
            var lista = await _context.Curriculos
                .Where(c => c.Nome.ToLower() == nome.ToLower())
                .ToListAsync();

            if (lista == null || lista.Count == 0)
                return NotFound("Nenhum currículo encontrado com esse nome.");

            return Ok(lista);
        }

        // POST: api/v1/curriculos
        [HttpPost]
        public async Task<ActionResult<Curriculo>> PostCurriculo(CurriculoCreateDto dto)
        {
            var curriculo = new Curriculo
            {
                Nome = dto.Nome,
                Email = dto.Email,
                Competencias = dto.Competencias,
                Experiencias = dto.Experiencias
            };

            _context.Curriculos.Add(curriculo);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetCurriculo), new { id = curriculo.Id }, curriculo);
        }

        // PUT: api/v1/curriculos/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutCurriculo(int id, CurriculoCreateDto dto)
        {
            var curriculoDb = await _context.Curriculos.FindAsync(id);

            if (curriculoDb == null)
                return NotFound("Currículo não encontrado.");

            curriculoDb.Nome = dto.Nome;
            curriculoDb.Email = dto.Email;
            curriculoDb.Competencias = dto.Competencias;
            curriculoDb.Experiencias = dto.Experiencias;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/curriculos/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteCurriculo(int id)
        {
            var curriculo = await _context.Curriculos.FindAsync(id);

            if (curriculo == null)
                return NotFound();

            _context.Curriculos.Remove(curriculo);
            await _context.SaveChangesAsync();

            return NoContent();
        }
    }
}
