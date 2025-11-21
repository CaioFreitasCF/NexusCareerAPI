using Microsoft.AspNetCore.Mvc;
using NexusCareerAPI.Data;
using NexusCareerAPI.DTOs;
using NexusCareerAPI.Models;

namespace NexusCareerAPI.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CandidatosController : ControllerBase
    {
        private readonly AppDbContext _db;

        public CandidatosController(AppDbContext db)
        {
            _db = db;
        }

        // POST: api/candidatos
        [HttpPost]
        public IActionResult CriarCandidato(CandidatoDto dto)
        {
            var candidato = new Candidato
            {
                Nome = dto.Nome
            };

            _db.Candidatos.Add(candidato);
            _db.SaveChanges();

            return CreatedAtAction(nameof(ObterCandidato), new { id = candidato.Id }, candidato);
        }

        // GET: api/candidatos
        [HttpGet]
        public ActionResult<IEnumerable<Candidato>> ObterTodos()
        {
            return Ok(_db.Candidatos.ToList());
        }

        // GET: api/candidatos/{id}
        [HttpGet("{id}")]
        public ActionResult<Candidato> ObterCandidato(int id)
        {
            var candidato = _db.Candidatos.Find(id);

            if (candidato == null)
                return NotFound(new { message = "Candidato não encontrado" });

            return Ok(candidato);
        }

        [HttpPut("{id}")]
        public IActionResult AtualizarCandidato(int id, CandidatoDto dto)
        {
            var candidato = _db.Candidatos.Find(id);

            if (candidato == null)
                return NotFound(new { message = "Candidato não encontrado" });

            candidato.Nome = dto.Nome;
            _db.SaveChanges();

            return NoContent();
        }

        [HttpDelete("{id}")]
        public IActionResult DeletarCandidato(int id)
        {
            var candidato = _db.Candidatos.Find(id);

            if (candidato == null)
                return NotFound(new { message = "Candidato não encontrado" });

            _db.Candidatos.Remove(candidato);
            _db.SaveChanges();

            return NoContent();
        }

        // GET: api/candidatos/{id}/vagas-ideais
        [HttpGet("{id}/vagas-ideais")]
        public IActionResult ObterVagasIdeais(int id)
        {
            var candidato = _db.Candidatos.Find(id);

            if (candidato == null)
                return NotFound(new { message = "Candidato não encontrado" });

          
            var curriculo = _db.Curriculos
                .FirstOrDefault(c => c.Nome.ToLower() == candidato.Nome.ToLower());

            if (curriculo == null)
                return NotFound(new { message = "Currículo não encontrado para este candidato" });

            if (string.IsNullOrWhiteSpace(curriculo.Competencias))
                return Ok(new { message = "Nenhuma competência cadastrada no currículo", vagas = new List<object>() });

           
            var competencias = curriculo.Competencias
                .ToLower()
                .Split(',', StringSplitOptions.RemoveEmptyEntries)
                .Select(s => s.Trim())
                .ToList();

           
            var vagas = _db.Vaga.ToList();

            
            var vagasCompatíveis = vagas
                .Where(v =>
                    !string.IsNullOrWhiteSpace(v.Requisitos) &&
                    competencias.Any(c => v.Requisitos.ToLower().Contains(c))
                )
                .ToList();

            return Ok(new
            {
                candidato = candidato.Nome,
                competencias = competencias,
                vagas = vagasCompatíveis
            });
        }
    }
}
