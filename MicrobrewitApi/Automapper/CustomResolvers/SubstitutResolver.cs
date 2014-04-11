﻿using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class SubstitutResolver : ValueResolver<HopDto, IList<Hop>>
    {
        protected override IList<Hop> ResolveCore(HopDto dto)
        {
            using (var context = new MicrobrewitContext())
            {
                List<Hop> hops = new List<Hop>();
                if (dto.Substituts != null)
                {
                    foreach (var sub in dto.Substituts)
                    {

                        if (sub.Id > 0)
                        {
                            var hop = context.Hops.SingleOrDefault(h => h.Id == sub.Id);
                            if (hop != null)
                            {
                                hops.Add(hop);
                            }
                        }
                        else
                        {
                            var hop = context.Hops.SingleOrDefault(h => h.Name.Equals(sub.Name));
                            if (hop != null)
                            {
                                hops.Add(hop);
                            }
                        }
                    }
                }
                return hops;
            }
        }
    }
}