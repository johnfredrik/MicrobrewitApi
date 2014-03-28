using AutoMapper;
using Microbrewit.Api.DTOs;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class HopOriginResolver : ValueResolver<DTO, Origin>
    {
        protected override Origin ResolveCore(DTO dto)
        {
            using (var context = new MicrobrewitContext())
            {
                Origin origin = null;
                if (dto != null)
                {
                    if (dto.Id != null)
                    {
                        origin = context.Origins.SingleOrDefault(o => o.Id == dto.Id);
                    }
                    else
                    {
                        origin = context.Origins.SingleOrDefault(o => o.Name == dto.Name);
                    }
                }
               return origin;
            }

        }
    }
}