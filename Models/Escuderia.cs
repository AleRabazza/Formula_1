using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Escuderia
    {
        [Key]
        public int IdEscuderia { get; set; }

        public int PuntajeAcumulado { get; set; }

        public string Nombre { get; set; }

        public string PaisDeOrigen { get; set; }

        public string SponsorPrincipal { get; set; }

        public List<Piloto> pilotos { get; set; }

        public int CantidadDePilotos { get; set; }
        public Escuderia() { }

        public Escuderia(string nombre, string paisDeOrigen, string sponsorPrincipal, int puntajeAcumulado)
        {

            Nombre = nombre;
            PaisDeOrigen = paisDeOrigen;
            SponsorPrincipal = sponsorPrincipal;
            PuntajeAcumulado = puntajeAcumulado;


        }
        public bool Validacion()
        {
            if(string.IsNullOrWhiteSpace(PaisDeOrigen) || string.IsNullOrEmpty(Nombre) || string.IsNullOrEmpty(SponsorPrincipal) || int.IsNegative(PuntajeAcumulado)){
                return false;
            }
           return true;
        }
        
        
    } 
}
