using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MicrobrewitModel;
using System.Linq.Expressions;

namespace MicrobrewitApi.DTOs
{
    public class RecipeDto
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<RecipeHopDto> Hops { get; set; }
    }
}