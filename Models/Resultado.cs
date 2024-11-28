using System.ComponentModel.DataAnnotations;

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

        public Resultado(int idCarrera, int idPiloto, int posicionSalida, int posicionLlegada)
        {
            IdCarrera = idCarrera;
            IdPiloto = idPiloto;
            PosicionSalida = posicionSalida;
            PosicionLlegada = posicionLlegada;
        }
    }
}
