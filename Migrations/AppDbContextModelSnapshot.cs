﻿// <auto-generated />
using System;
using Formula_1.Data;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Storage.ValueConversion;

#nullable disable

namespace Formula_1.Migrations
{
    [DbContext(typeof(AppDbContext))]
    partial class AppDbContextModelSnapshot : ModelSnapshot
    {
        protected override void BuildModel(ModelBuilder modelBuilder)
        {
#pragma warning disable 612, 618
            modelBuilder
                .HasAnnotation("ProductVersion", "9.0.0")
                .HasAnnotation("Relational:MaxIdentifierLength", 128);

            SqlServerModelBuilderExtensions.UseIdentityColumns(modelBuilder);

            modelBuilder.Entity("Formula_1.Models.Carrera", b =>
                {
                    b.Property<int>("IdCarrera")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdCarrera"));

                    b.Property<string>("Ciudad")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<DateOnly>("fecha")
                        .HasColumnType("date");

                    b.HasKey("IdCarrera");

                    b.ToTable("Carreras");
                });

            modelBuilder.Entity("Formula_1.Models.Escuderia", b =>
                {
                    b.Property<int>("IdEscuderia")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdEscuderia"));

                    b.Property<string>("Nombre")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaisDeOrigen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PuntajeAcumulado")
                        .HasColumnType("int");

                    b.Property<string>("SponsorPrincipal")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.HasKey("IdEscuderia");

                    b.ToTable("Escuderia");
                });

            modelBuilder.Entity("Formula_1.Models.Piloto", b =>
                {
                    b.Property<int>("NumeroPiloto")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("NumeroPiloto"));

                    b.Property<DateOnly>("FechaNac")
                        .HasColumnType("date");

                    b.Property<int>("IdEscuderia")
                        .HasColumnType("int");

                    b.Property<string>("NombrePiloto")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<string>("PaisDeOrigen")
                        .IsRequired()
                        .HasColumnType("nvarchar(max)");

                    b.Property<int>("PuntajeAcumulado")
                        .HasColumnType("int");

                    b.HasKey("NumeroPiloto");

                    b.HasIndex("IdEscuderia");

                    b.ToTable("Pilotos");
                });

            modelBuilder.Entity("Formula_1.Models.Resultado", b =>
                {
                    b.Property<int>("IdResultado")
                        .ValueGeneratedOnAdd()
                        .HasColumnType("int");

                    SqlServerPropertyBuilderExtensions.UseIdentityColumn(b.Property<int>("IdResultado"));

                    b.Property<int>("PosicionLlegada")
                        .HasColumnType("int");

                    b.Property<int>("PosicionSalida")
                        .HasColumnType("int");

                    b.Property<int>("carreraIdCarrera")
                        .HasColumnType("int");

                    b.Property<int>("pilotoNumeroPiloto")
                        .HasColumnType("int");

                    b.HasKey("IdResultado");

                    b.HasIndex("carreraIdCarrera");

                    b.HasIndex("pilotoNumeroPiloto");

                    b.ToTable("Resultados");
                });

            modelBuilder.Entity("Formula_1.Models.Piloto", b =>
                {
                    b.HasOne("Formula_1.Models.Escuderia", "escuderia")
                        .WithMany("pilotos")
                        .HasForeignKey("IdEscuderia")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("escuderia");
                });

            modelBuilder.Entity("Formula_1.Models.Resultado", b =>
                {
                    b.HasOne("Formula_1.Models.Carrera", "carrera")
                        .WithMany()
                        .HasForeignKey("carreraIdCarrera")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.HasOne("Formula_1.Models.Piloto", "piloto")
                        .WithMany()
                        .HasForeignKey("pilotoNumeroPiloto")
                        .OnDelete(DeleteBehavior.Cascade)
                        .IsRequired();

                    b.Navigation("carrera");

                    b.Navigation("piloto");
                });

            modelBuilder.Entity("Formula_1.Models.Escuderia", b =>
                {
                    b.Navigation("pilotos");
                });
#pragma warning restore 612, 618
        }
    }
}
