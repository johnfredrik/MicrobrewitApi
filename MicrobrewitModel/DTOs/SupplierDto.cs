using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class SupplierDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public DTO Origin { get; set; }


    }
}