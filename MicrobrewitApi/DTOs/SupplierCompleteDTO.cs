using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class SupplierCompleteDTO
    {
        public Links Links { get; set; }
        public IList<Supplier> Suppliers { get; set; }

        public SupplierCompleteDTO()
        {
            Links = new Links()
            {
                Href = "http://api.microbrew.it/origins/:id",
                Type = "origin"
            };
        }
    }
}