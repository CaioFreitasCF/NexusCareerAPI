using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using NexusCareerAPI.Data;
using NexusCareerAPI.Models;
using NexusCareerAPI.DTOs;

namespace NexusCareerAPI.Controllers.V1
{
    [ApiController]
    [Route("api/v1/[controller]")]
    public class EmpresasController : ControllerBase
    {
        private readonly AppDbContext _context;

        public EmpresasController(AppDbContext context)
        {
            _context = context;
        }

        // GET: api/v1/empresas
        [HttpGet]
        public async Task<ActionResult<IEnumerable<Empresa>>> GetEmpresas()
        {
            return Ok(await _context.Empresas.ToListAsync());
        }

        // GET: api/v1/empresas/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Empresa>> GetEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            return Ok(empresa);
        }

        // POST: api/v1/empresas
        [HttpPost]
        public async Task<ActionResult<Empresa>> PostEmpresa(EmpresaDto dto)
        {
            var empresa = new Empresa
            {
                Nome = dto.Nome
            };

            _context.Empresas.Add(empresa);
            await _context.SaveChangesAsync();

            return CreatedAtAction(nameof(GetEmpresa), new { id = empresa.Id }, empresa);
        }

        // PUT: api/v1/empresas/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutEmpresa(int id, EmpresaDto dto)
        {
            var empresaDb = await _context.Empresas.FindAsync(id);

            if (empresaDb == null)
                return NotFound("Empresa não encontrada.");

            empresaDb.Nome = dto.Nome;

            await _context.SaveChangesAsync();
            return NoContent();
        }

        // DELETE: api/v1/empresas/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteEmpresa(int id)
        {
            var empresa = await _context.Empresas.FindAsync(id);

            if (empresa == null)
                return NotFound();

            _context.Empresas.Remove(empresa);
            await _context.SaveChangesAsync();

            return NoContent();
        }

        // GET: api/v1/empresas/{nome}/vagas  ← novo endpoint
        [HttpGet("{nome}/vagas")]
        public async Task<ActionResult<IEnumerable<Vagas>>> GetVagasPorEmpresa(string nome)
        {
            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower());

            if (empresa == null)
                return NotFound("Empresa não encontrada.");

            var vagas = await _context.Vaga
                .Where(v => v.Empresa.ToLower() == nome.ToLower())
                .ToListAsync();

            return Ok(vagas);
        }

        // GET: api/v1/empresas/{nome}/curriculos-ideais
        [HttpGet("{nome}/curriculos-ideais")]
        public async Task<IActionResult> GetCurriculosIdeais(string nome)
        {
            if (string.IsNullOrWhiteSpace(nome))
                return BadRequest(new { message = "Nome da empresa é obrigatório." });

           
            var empresa = await _context.Empresas
                .FirstOrDefaultAsync(e => e.Nome.ToLower() == nome.ToLower());

            if (empresa == null)
                return NotFound(new { message = "Empresa não encontrada." });

            
            var vagas = await _context.Vaga
                .Where(v => v.Empresa.ToLower() == empresa.Nome.ToLower())
                .ToListAsync();

            if (!vagas.Any())
                return Ok(new
                {
                    empresa = empresa.Nome,
                    message = "Nenhuma vaga encontrada.",
                    resultados = new List<object>()
                });

           
            var curriculos = await _context.Curriculos.ToListAsync();

            var resultados = new List<object>();

            foreach (var vaga in vagas)
            {
                if (string.IsNullOrWhiteSpace(vaga.Requisitos))
                {
                    resultados.Add(new
                    {
                        vaga = vaga.Titulo,
                        requisitos = "Nenhum requisito informado",
                        curriculosCompatíveis = new List<object>()
                    });
                    continue;
                }

                var requisitos = vaga.Requisitos
                    .ToLower()
                    .Split(',', StringSplitOptions.RemoveEmptyEntries)
                    .Select(s => s.Trim())
                    .ToList();

                var curriculosMatch = curriculos
                    .Where(c =>
                        !string.IsNullOrWhiteSpace(c.Competencias) &&
                        requisitos.Any(req => c.Competencias.ToLower().Contains(req))
                    )
                    .Select(c => new
                    {
                        c.Id,
                        c.Nome,
                        c.Email,
                        c.Competencias
                    })
                    .ToList();

                resultados.Add(new
                {
                    vaga = vaga.Titulo,
                    requisitos = requisitos,
                    curriculosCompatíveis = curriculosMatch
                });
            }

            return Ok(new
            {
                empresa = empresa.Nome,
                resultados
            });
        }

    }
}
