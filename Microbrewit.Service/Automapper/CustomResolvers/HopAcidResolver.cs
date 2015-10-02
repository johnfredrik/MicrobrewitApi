using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class HopAcidResolver : ValueResolver<Hop, AcidDto>
    {
        protected override AcidDto ResolveCore(Hop hop)
        {
            var acid = new AcidDto
            {
                AlphaAcid = new AlphaAcidDto
                {
                    Low = hop.AALow,
                    High = hop.AAHigh
                },
                BetaAcid = new BetaAcidDto
                {
                    High = hop.BetaHigh,
                    Low = hop.BetaLow,
                }
            };
            return acid;
        }
    }
}