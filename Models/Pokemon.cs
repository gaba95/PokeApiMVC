using System.Collections.Generic;

namespace PokeApiMvc.Models
{
    public class Pokemon
    {
        public string Name { get; set; }
        public double Height { get; set; }
        public double Weight { get; set; }
        public string ImageUrl { get; set; }
        public List<string> Types { get; set; }
    }
}
