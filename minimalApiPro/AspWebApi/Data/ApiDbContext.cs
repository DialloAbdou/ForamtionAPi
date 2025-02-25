using AspWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspWebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public DbSet<Personne> Personnes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personne>(p =>
            {
                p.ToTable("Personnes");
                p.Property(p => p.Nom).HasMaxLength(250);
                p.Property(p=>p.Prenom).HasMaxLength(50);
            });
        }

        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("Filename=api.db");
        }
    }
}
