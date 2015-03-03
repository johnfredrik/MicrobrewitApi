using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using System.ComponentModel.DataAnnotations;
using Newtonsoft.Json;
using Nest;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "beerStyle")]
    public class BeerStyleSimpleDto
    {
        [JsonProperty(PropertyName = "beerStyleId")]
        public int Id { get; set; }
        [JsonProperty(PropertyName = "name")]
        public string Name { get; set; }
       
    }
}