using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class SetHopFromNullResolver : ValueResolver<HopStepDto,HopForm>
    {
        protected override HopForm ResolveCore(HopStepDto source)
        {
            return null;
        }
    }
}