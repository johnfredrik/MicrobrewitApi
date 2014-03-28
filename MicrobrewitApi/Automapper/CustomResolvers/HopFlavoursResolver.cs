using AutoMapper;
using Microbrewit.Api.DTOs;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class HopFlavoursResolver : ValueResolver<DTO, Flavour>
    {
        protected override Flavour ResolveCore(DTO dto)
        {
            using (var context = new MicrobrewitContext())
            {
                Flavour flavour = null;
                if (dto != null)
                {
                    if (dto.Id > 0)
                    {
                        flavour = context.Flavours.SingleOrDefault(f => f.Id == dto.Id);
                    }
                    else
                    {
                        flavour = context.Flavours.SingleOrDefault(f => f.Name.Equals(dto.Name));
                    }
                    if (flavour == null)
                    {
                        flavour = new Flavour() { Name = dto.Name };
                        context.Flavours.Add(flavour);
                    }
                }
                return flavour;
            }

        }
    }
}