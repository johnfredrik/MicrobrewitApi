using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class LinksFermentable
    {
        [JsonProperty(PropertyName = "fermentables.maltster")]
        public Links FermentablesMaltster { get; set; }
     

    }
}