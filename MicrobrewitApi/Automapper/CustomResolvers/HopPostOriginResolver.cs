using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class HopPostOriginResolver : ValueResolver<HopPostDto, int>
    {
        protected override int ResolveCore(HopPostDto dto)
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