using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Piloto
    {
        [Key]
        public int NumeroPiloto { get; set; }

        public string NombrePiloto { get; set; }

        public DateOnly FechaNac {  get; set; }

        public string PaisDeOrigen {  get; set; }

        public Escuderia escuderia { get; set; }

        public int PuntajeAcumulado { get; set; }

        public int IdEscuderia { get; set; }

        public Piloto()
        {
        }

        public Piloto(int numeroPiloto, string nombrePiloto, DateOnly fechaNac, string paisDeOrigen, Escuderia escuderia, int puntajeAcumulado)
        {
            NumeroPiloto = numeroPiloto;
            NombrePiloto = nombrePiloto;
            FechaNac = fechaNac;
            PaisDeOrigen = paisDeOrigen;
            this.escuderia = escuderia;
            PuntajeAcumulado = puntajeAcumulado;
        }
    }
}
