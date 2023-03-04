using System.Collections.Generic;

namespace LogCorrida.Models
{
    public class Piloto
    {
        public Piloto()
        {
            Voltas = new List<Volta>();
        }

        public string Codigo { get; set; }

        public string Nome { get; set; }

        public List<Volta> Voltas { get; set; }
    }
}
