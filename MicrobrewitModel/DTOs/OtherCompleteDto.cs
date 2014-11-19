using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class OtherCompleteDto
    {
        [JsonProperty(PropertyName = "links")]
        public LinksOther Links { get; set; }
        [JsonProperty(PropertyName = "others")]
        public IList<OtherDto> Others { get; set; }
    }
}