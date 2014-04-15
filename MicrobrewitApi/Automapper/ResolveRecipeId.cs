using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model.DTOs;
using System.Reflection;

namespace Microbrewit.Api.Automapper
{
    public class ResolveRecipeId<T> : ValueResolver<T,int>
    {
        protected override int ResolveCore(T source)
        {
            using (var context = new Microbrewit.Model.MicrobrewitContext())
            {
                return 1;
            }
        }
    }
}