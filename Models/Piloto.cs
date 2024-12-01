using Microsoft.IdentityModel.Tokens;
using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Piloto
    {
        [Key]
        public int NumeroPiloto { get; set; }
        public int NumeroAuto { get; set; }

        public string NombrePiloto { get; set; }

        public DateOnly FechaNac {  get; set; }

        public string PaisDeOrigen {  get; set; }

        public int PuntajeAcumulado { get; set; }
        public int EscuderiaId {  get; set; }
        public Escuderia Escuderia { get; set; }

        public List<Resultado> Resultados { get; set; }

        public Piloto()
        {
        }

        public Piloto(int numeroAuto, string nombrePiloto, DateOnly fechaNac, string paisDeOrigen, Escuderia escuderia)
        {
            NumeroAuto = numeroAuto;
            NombrePiloto = nombrePiloto;
            FechaNac = fechaNac;
            PaisDeOrigen = paisDeOrigen;
            this.Escuderia = escuderia;
            EscuderiaId = (escuderia.IdEscuderia);
            PuntajeAcumulado = 0;
            Resultados = new List<Resultado>();
        }

        public bool Validacion()
        {
            if (string.IsNullOrEmpty(NombrePiloto) || string.IsNullOrEmpty(PaisDeOrigen))
            {
                return false;
            }

            return true;
                 
            }
        }
    }

