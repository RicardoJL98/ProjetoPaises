
namespace ProjetoPaises.Modelos
{

    public class Pais
    {
        public string Name { get; set; }

        public string Capital { get; set; }

        public string Region { get; set; }

        public string SubRegion { get; set; }

        public int Population { get; set; }

        public string Gini { get; set; }

        public string Flag { get; set; }

        public override string ToString()
        {
            return $"{Name}";
        }
    }
}
