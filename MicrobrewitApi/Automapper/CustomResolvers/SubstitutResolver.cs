using AutoMapper;
using Microbrewit.Api.DTOs;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class SubstitutResolver : ValueResolver<DTO,Hop>
    {
        protected override Hop ResolveCore(DTO dto)
        {
            using (var context = new MicrobrewitContext())
            {
                Hop hop = null;
                if(dto != null)
                    {
                    if (dto.Id > 0)
                    {
                        hop = context.Hops.SingleOrDefault(h => h.Id == dto.Id);
                    }
                    else
                    {
                        hop = context.Hops.SingleOrDefault(h => h.Name.Equals(dto.Name));
                    }
                }
                return hop;
            }
        }
    }
}