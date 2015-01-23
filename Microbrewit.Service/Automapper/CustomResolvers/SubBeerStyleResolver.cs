using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;

namespace Microbrewit.Service.Automapper.CustomResolvers
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