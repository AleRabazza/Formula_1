﻿using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Carrera
    {
        [Key]
        public int IdCarrera { get; set; }

        public string Nombre { get; set; }

        public string Ciudad  { get; set; }

        public DateOnly fecha { get; set; }

        public List<Resultado> Resultados { get; set; }

        public Carrera() { }

        public Carrera(string nombre, string ciudad, DateOnly fecha)
        {
            Nombre = nombre;
            Ciudad = ciudad;
            this.fecha = fecha;
            Resultados = new List<Resultado>();
        }
    }
}
