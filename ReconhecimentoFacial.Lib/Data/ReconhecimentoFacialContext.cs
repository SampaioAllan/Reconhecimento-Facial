using Microsoft.EntityFrameworkCore;
using ReconhecimentoFacial.Lib.Models;
namespace ReconhecimentoFacial.Lib.Data.Repositorios
{
    public class ReconhecimentoFacialContext : DbContext
    {
        public ReconhecimentoFacialContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);
            //mapeando tabelas

            //Usuario
            modelBuilder.Entity<Usuario>().ToTable("reconhecimento_facial_usuarios");
            modelBuilder.Entity<Usuario>().HasKey(key => key.Id);
        }
        public DbSet<Usuario> Usuarios { get; set; }
    }
}