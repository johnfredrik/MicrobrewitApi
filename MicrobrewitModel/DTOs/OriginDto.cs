using Nest;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "origin")]
    public class OriginDto
    {
        [JsonProperty(PropertyName = "otherId")]
        public int Id { get; set; }
        public string Name { get; set; }
        public string DataType { get { return "origin"; } }
    }
}
