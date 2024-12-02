using Formula_1.Models; 
using Microsoft.EntityFrameworkCore;

namespace Formula_1.Data
{
    public class AppDbContext : DbContext
    {
        public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

        public DbSet<Piloto> Pilotos { get; set; }

        public DbSet<Carrera> Carreras { get; set; }

        public DbSet<Resultado> Resultados { get; set; }

        public DbSet<Escuderia> Escuderia { get; set; }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Piloto>()
                .HasKey(piloto => piloto.NumeroPiloto);

            modelBuilder.Entity<Carrera>()
                .HasKey(carrera => carrera.IdCarrera);

            modelBuilder.Entity<Escuderia>()
                .HasKey(escuderia => escuderia.IdEscuderia);

            modelBuilder.Entity<Resultado>()
                .HasKey(resultado => resultado.IdResultado);

           
            modelBuilder.Entity<Piloto>()
                .HasOne(p => p.Escuderia)
                .WithMany(e => e.Pilotos)
                .HasForeignKey(p => p.EscuderiaId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Resultado>()
             .HasOne(r => r.Carrera)
             .WithMany(c => c.Resultados)
             .HasForeignKey(r => r.IdCarrera)
             .OnDelete(DeleteBehavior.Cascade);  

            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Piloto)
                .WithMany(p => p.Resultados)
                .HasForeignKey(r => r.IdPiloto)
                .OnDelete(DeleteBehavior.Cascade);
        }
    }
}