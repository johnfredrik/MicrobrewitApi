using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class BeerSimpleDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public ABV ABV { get; set; }
        public IBU IBU { get; set; }
        public SRM SRM { get; set; }
        public DTO Recipe { get; set; }

        public IList<DTO> Breweries { get; set; }
        public IList<DTO> Brewers { get; set; }

    }
}