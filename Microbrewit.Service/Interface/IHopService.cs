using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IHopService
    {
        Task<IList<HopDto>> GetHopsAsync();
        Task<HopDto> GetHopAsync();

    }
}
