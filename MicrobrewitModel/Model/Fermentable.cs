using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Web;

namespace MicrobrewitModel
{
    public class Fermentable
    {
        
        public int Id { get; set; }
        public string Name { get; set; }
        public int Colour { get; set; }      
        public int PPG { get; set; }
        public int TypeId { get; set; }
        public virtual FermentableType Type { get; set; }

        public Nullable<int> SupplierId { get; set; }
        public virtual Supplier Supplier { get; set; }
    }
}