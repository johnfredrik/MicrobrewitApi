using Microbrewit.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.DTOs
{
    public class GlassCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        public Links Links { get; set; }
        public IList<GlassDto> Glasses { get; set; }
    }
}
