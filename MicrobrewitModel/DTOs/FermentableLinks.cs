using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class FermentableLinks
    {
        [JsonProperty(PropertyName = "maltsterid")]
        public int MaltsterId { get; set; }
    }
}