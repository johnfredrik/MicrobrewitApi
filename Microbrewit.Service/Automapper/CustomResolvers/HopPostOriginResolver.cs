using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class HopPostOriginResolver : ValueResolver<HopDto, int>
    {
        protected override int ResolveCore(HopDto dto)
        {
            using (var context = new MicrobrewitContext())
            {
                Origin origin = null;
                if (dto.Origin != null)
                {
                    origin = context.Origins.SingleOrDefault(o => o.Id == dto.Origin.Id || o.Name.Equals(dto.Origin.Name));

                    if (origin == null)
                    {
                        origin = new Origin() { Name = dto.Origin.Name };
                        context.Origins.Add(origin);
                        context.SaveChanges();

                    }
                }
               return origin.Id;
            }

        }
    }
}