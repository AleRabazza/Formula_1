using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Escuderia
    {
        [Key]
        public int IdEscuderia {  get; set; }

        public int PuntajeAcumulado { get; set; }

        public string Nombre { get; set; }

        public string PaisDeOrigen {  get; set; }

        public string SponsorPrincipal { get; set; }
    }
}
