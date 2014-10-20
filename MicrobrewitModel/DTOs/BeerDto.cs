using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class BeerDto
    {       
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            public ABVDto ABV { get; set; }
            public IBUDto IBU { get; set; }
            public SRMDto SRM { get; set; }
            public DTO BeerStyle { get; set; }
            public RecipeDto Recipe { get; set; }

            public IList<DTO> Breweries { get; set; }
            public IList<DTOUser> Brewers { get; set; }
            public string DataType { get { return "beer"; } }
       
    }
}
