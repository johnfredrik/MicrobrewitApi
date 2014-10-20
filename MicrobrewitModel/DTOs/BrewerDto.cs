using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class BrewerDto
    {
        public string Id { get; set; }
        public string BreweryName { get; set; }
        public string DataType { get { return "brewer"; } }
    }

}