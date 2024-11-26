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
    }
}
