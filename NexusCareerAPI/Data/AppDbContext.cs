using Microsoft.EntityFrameworkCore;
using NexusCareerAPI.Models;

namespace NexusCareerAPI.Data
{
    public class AppDbContext : DbContext   
    {
        public AppDbContext(DbContextOptions<AppDbContext> options)
            : base(options) { }            

        public DbSet<Curriculo> Curriculos { get; set; }
        public DbSet<Vagas> Vaga { get; set; }

        public DbSet<Empresa> Empresas { get; set; }
        public DbSet<Candidato> Candidatos { get; set; }




    }
}
