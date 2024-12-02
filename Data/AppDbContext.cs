﻿using Formula_1.Models; 
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
                .HasOne(p => p.Escuderia)  // Un Piloto tiene una Escuderia
                .WithMany(e => e.Pilotos)  // Una Escuderia puede tener muchos Pilotos
                .HasForeignKey(p => p.EscuderiaId)
                .OnDelete(DeleteBehavior.Cascade);  // La clave foránea en Piloto que referencia a Escuderia

            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Carrera)
                .WithMany(c => c.Resultados)
                .HasForeignKey(r => r.IdCarrera)
                .OnDelete(DeleteBehavior.SetNull);
            

            modelBuilder.Entity<Resultado>()
                .HasOne(r => r.Piloto)
                .WithMany(p => p.Resultados)
                .HasForeignKey(r => r.IdPiloto)
                .OnDelete(DeleteBehavior.SetNull);


            modelBuilder.Entity<Carrera>()
                .HasMany(c => c.Resultados)
                .WithOne(r => r.Carrera)
                .HasForeignKey(r => r.IdCarrera)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Piloto>()
                .HasMany(p => p.Resultados)
                .WithOne(r => r.Piloto)
                .HasForeignKey(r => r.IdCarrera)
                .OnDelete(DeleteBehavior.Cascade);

        }            
     }
}
