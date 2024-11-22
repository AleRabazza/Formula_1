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

        public List<Piloto> pilotos { get; set; }
        public Escuderia() { }

        public Escuderia( int puntajeAcumulado, string nombre, string paisDeOrigen, string sponsorPrincipal)
        {
            PuntajeAcumulado = puntajeAcumulado;
            Nombre = nombre;
            PaisDeOrigen = paisDeOrigen;
            SponsorPrincipal = sponsorPrincipal;
        }
    }
}
