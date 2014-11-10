using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "brewery")]
    public class BreweryDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public string Type { get; set; }
        public string DataType { get { return "brewery"; } }

        public IList<DTOUser> Members { get; set; }
        public IList<DTO> Beers { get; set; }
    }
}
