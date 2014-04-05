using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;


namespace Microbrewit.Model.DTOs
{
    public class FermentablePostDto
    {
        public int Id { get; set; }
        public DTO Supplier { get; set; }
        public string Name { get; set; }
        public double? EBC { get; set; }
        public int? PPG { get; set; }
        public string Type { get; set; }
    }

}