using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class FermentablesCompleteDto
    {
        //public Meta Meta { get; set; }
        [JsonProperty(PropertyName = "links")]
        public LinksFermentable Links { get; set; }
        [JsonProperty(PropertyName = "fermentables")]
        public ICollection<FermentableDto> Fermentables { get; set; }

        public FermentablesCompleteDto()
        {
            Links = new LinksFermentable()
            {
                
                FermentablesMaltster = new Links()
                {
                    Href = "http://api.microbrew.it/maltsters/:id",
                    Type = "maltster",
                }
            };
        }
    }
}