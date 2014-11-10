using Nest;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    [ElasticType(Name = "glass")]
    public class GlassDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }
}
