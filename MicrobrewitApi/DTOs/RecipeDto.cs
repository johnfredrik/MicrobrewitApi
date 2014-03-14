using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using Microbrewit.Model;
using System.Linq.Expressions;

namespace Microbrewit.Api.DTOs
{
    public class RecipeDto
    {
        public virtual int Id { get; set; }
        public virtual string Name { get; set; }
        public virtual ICollection<RecipeHopDto> Hops { get; set; }
    }
}