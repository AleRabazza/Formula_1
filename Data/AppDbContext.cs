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
            // Configuramos la relación "Uno a Muchos" desde Piloto a Escuderia
            modelBuilder.Entity<Piloto>()
                .HasOne(p => p.escuderia)  // Un Piloto tiene una Escuderia
                .WithMany(e => e.pilotos)  // Una Escuderia puede tener muchos Pilotos
                .HasForeignKey(p => p.IdEscuderia);  // La clave foránea en Piloto que referencia a Escuderia
        }


    }
}
