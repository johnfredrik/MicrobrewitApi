using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class SupplierCompleteDto
    {
        public Links Links { get; set; }
        public IList<SupplierDto> Suppliers { get; set; }

        public SupplierCompleteDto()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/origins/:id",
                Type = "origin"
            };
        }
    }
}