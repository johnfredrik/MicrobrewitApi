using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class SubBeerStyleResolver : ValueResolver<BeerStyle, IList<string>>
    {
        protected override IList<string> ResolveCore(BeerStyle BeerStyle)
        {
            var subStyles = new List<string>();
            if (BeerStyle.SubStyles != null)
            {
                foreach (var subStyle in BeerStyle.SubStyles)
                {
                    subStyles.Add(subStyle.Name);
                }
            }
            return subStyles;
        }

    }
}