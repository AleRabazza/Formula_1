using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Components.Web;

namespace Formula_1.Models
{
    public class Resultado
    {
        [Key]
        public int IdResultado {  get; set; }

        public int IdCarrera { get; set; }

        public Carrera Carrera { get; set; }

        public int IdPiloto { get; set; }

        public Piloto Piloto { get; set; }

        public int PosicionSalida { get; set; }

        public int PosicionLlegada { get; set; }

        public Resultado() { }

        public Resultado(Carrera carrera, Piloto piloto, int posicionSalida, int posicionLlegada)
        {
            IdCarrera = carrera.IdCarrera;
            IdPiloto = piloto.NumeroPiloto;
            PosicionSalida = posicionSalida;
            PosicionLlegada = posicionLlegada;
            Piloto = piloto;
            Carrera = carrera;
        }

        public bool Verificar()
        {
            if (Carrera == null || Piloto == null)
            {
                return false;
            }
            return true;
        }
    }
}
