using System.ComponentModel.DataAnnotations;

namespace Formula_1.Models
{
    public class Resultado
    {
        [Key]
        public int IdResultado {  get; set; }

        public Carrera carrera { get; set; }

        public Piloto piloto { get; set; }

        public int PosicionSalida { get; set; }

        public int PosicionLlegada { get; set; }
    }
}
