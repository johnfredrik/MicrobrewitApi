using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class SetHopFromNullResolver : ValueResolver<HopStepDto,HopForm>
    {
        protected override HopForm ResolveCore(HopStepDto source)
        {
            return null;
        }
    }
}