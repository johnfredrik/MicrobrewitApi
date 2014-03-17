using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class FermentableStepDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public int Colour { get; set; }
        public int PPG { get; set; }
        public int SuppliedById { get; set; }
        public string Origin { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}