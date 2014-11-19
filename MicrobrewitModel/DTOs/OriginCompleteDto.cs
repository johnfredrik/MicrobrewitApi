using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class OriginCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        [JsonProperty(PropertyName = "links")]
        public Links Links { get; set; }
        [JsonProperty(PropertyName = "origins")]
        public IList<OriginDto> Origins { get; set; }
    }
}
