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
            [JsonProperty(PropertyName = "id")]
            public int Id { get; set; }
            [Required]
            [JsonProperty(PropertyName = "name")]
            public string Name { get; set; }
            [JsonProperty(PropertyName = "abv")]
            public ABVDto ABV { get; set; }
            [JsonProperty(PropertyName = "ibu")]
            public IBUDto IBU { get; set; }
            [JsonProperty(PropertyName = "srm")]
            public SRMDto SRM { get; set; }
            [JsonProperty(PropertyName = "beerStyle")]
            public DTO BeerStyle { get; set; }
            [JsonProperty(PropertyName = "recipe")]
            public RecipeDto Recipe { get; set; }
            [JsonProperty(PropertyName = "createdDate")]
            public DateTime CreatedDate { get; set; }
            [JsonProperty(PropertyName = "updatedDate")]
            public DateTime UpdatedDate { get; set; }
            [JsonProperty(PropertyName = "breweries")]
            public IList<DTO> Breweries { get; set; }
            [JsonProperty(PropertyName = "brewers")]
            public IList<DTOUser> Brewers { get; set; }
            [JsonProperty(PropertyName = "dataType")]
            public string DataType { get { return "beer"; } }
       
    }
}
