using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class SupplierCompleteDto
    {
        private static readonly string apiPath = ConfigurationManager.AppSettings["api"];
        public Links Links { get; set; }
        public IList<SupplierDto> Suppliers { get; set; }

        public SupplierCompleteDto()
        {
            Links = new Links()
            {
                Href = apiPath + "/origins/:id",
                Type = "origin"
            };
        }
    }
}