using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class BeerSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ABVDto ABV { get; set; }
        public IBUDto IBU { get; set; }
        public SRMDto SRM { get; set; }
        public DTO Recipe { get; set; }
        public string DataType { get{return "beer";} }

        public IList<DTO> Breweries { get; set; }
        public IList<DTOUser> Brewers { get; set; }

    }
}