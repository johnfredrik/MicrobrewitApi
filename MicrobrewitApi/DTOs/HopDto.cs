using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class HopDto
    {
        public int Id { get; set; }
        public string Href { get { return "http://api.microbrew.it/hops/:id"; }}
        public string Name { get; set; }
        public int AALow { get; set; }
        public int AAHigh { get; set; }
        public IList<string> Flavours { get; set; }
        public IList<String> Substitutions { get; set; }
        public HopLinks Links { get; set; }

        
    }
}