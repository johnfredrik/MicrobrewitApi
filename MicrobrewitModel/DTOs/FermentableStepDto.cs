using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class FermentableStepDto
    {
        public int FermentableId { get; set; }
        public int StepId { get; set; }
        public string Name { get; set; }
        public double Lovibond { get; set; }
        public double PPG { get; set; }
        public SupplierDto Supplier { get; set; }
        public string Type { get; set; }
        public int Amount { get; set; }
    }
}