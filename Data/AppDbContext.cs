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

    }
}
