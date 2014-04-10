using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;
using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class HopFlavoursResolver : ValueResolver<HopDto, IList<HopFlavour>>
    {
        private IHopRepository repository = new HopRepository();

        protected override IList<HopFlavour> ResolveCore(HopDto dto)
        {

            using (var context = new MicrobrewitContext())
            {
                var flavours = new List<HopFlavour>();
                if (dto.Flavours != null)
                {
                    foreach (var item in dto.Flavours)
                    {
                        var hopFlavor = new HopFlavour();
                        if (item.Id > 0)
                        {
                            hopFlavor.FlavourId = item.Id;
                            hopFlavor.HopId = dto.Id;
                            flavours.Add(hopFlavor);
                        }
                        else
                        {
                            var flavour = repository.AddFlavour(item.Name);
                            if (flavour == null)
                            {
                                flavour = 
                                context.Flavours.Add(flavour);
                                context.SaveChanges();
                            }
                            hopFlavor.FlavourId = flavour.Id;
                            flavours.Add(hopFlavor);
                        }
                    
                    }
                }
                return flavours;
            }

        }
    }
}