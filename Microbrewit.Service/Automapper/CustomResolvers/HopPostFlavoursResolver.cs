using System.Collections.Generic;
using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class HopPostFlavoursResolver : ValueResolver<HopDto, IList<HopFlavour>>
    {
        private IHopRepository repository = new HopDapperRepository();

        protected override IList<HopFlavour> ResolveCore(HopDto dto)
        {
            var hopFlavours = new List<HopFlavour>();
            var flavours = repository.GetFlavours();
            if (dto.Flavours != null)
            {
                foreach (var item in dto.Flavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.Name == item);
                    if (flavour != null)
                        hopFlavours.Add(new HopFlavour { FlavourId = flavour.FlavourId, HopId = dto.Id });
                }
            }
            return hopFlavours;
        }

    }
}
