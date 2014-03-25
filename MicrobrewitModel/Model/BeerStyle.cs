using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class BeerStyle
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int? SuperStyleId { get; set; }
        public BeerStyle SuperStyle { get; set; }
        public ICollection<BeerStyle> SubStyles { get; set; }

        public ICollection<Recipe> Recipes { get; set; }

    }
}
