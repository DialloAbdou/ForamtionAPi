using AspWebApi.Data.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Metadata;

namespace AspWebApi.Data
{
    public class ApiDbContext : DbContext
    {
        public ApiDbContext(DbContextOptions<ApiDbContext>options )
            :base(options)
        {
                
        }
        public DbSet<Personne> Personnes { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Personne>(p =>
            {
                p.ToTable("Personnes");
                p.Property(p => p.Nom).HasMaxLength(250);
                p.Property(p=>p.Prenom).HasMaxLength(50);
                p.Property(p=>p.DisplayId).HasMaxLength(16);
                p.HasIndex(p=>p.DisplayId).IsUnique();

            });
        }


    }
}
