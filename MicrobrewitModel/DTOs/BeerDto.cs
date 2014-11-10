using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "beer")]
    public class BeerDto
    {       
            public int Id { get; set; }
            [Required]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "abv")]
            public ABVDto ABV { get; set; }
            [JsonProperty(PropertyName = "ibu")]
            public IBUDto IBU { get; set; }
            [JsonProperty(PropertyName = "srm")]
            public SRMDto SRM { get; set; }
            public DTO BeerStyle { get; set; }
            public RecipeDto Recipe { get; set; }
            public DateTime CreatedDate { get; set; }
            public DateTime UpdatedDate { get; set; }

            public IList<DTO> Breweries { get; set; }
            public IList<DTOUser> Brewers { get; set; }
            public string DataType { get { return "beer"; } }
       
    }
}
