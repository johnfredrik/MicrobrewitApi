using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BeerStyleLinksResolver : ValueResolver<BeerStyle, BeerStyleLinks>
    {
        protected override BeerStyleLinks ResolveCore(BeerStyle beerStyle)
        {
            var beerStyleLinks = new BeerStyleLinks() { SubBeerStyleIds = new List<int>()};
            if (beerStyle.SubStyles != null)
            {
                foreach (var item in beerStyle.SubStyles)
                {
                    beerStyleLinks.SubBeerStyleIds.Add(item.BeerStyleId);
                }
            }
            return beerStyleLinks;
        }

    }
}