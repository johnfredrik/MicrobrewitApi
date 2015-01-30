using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IHopService
    {
        Task<IEnumerable<HopDto>> GetHopsAsync(string custom);
        Task<HopDto> GetHopAsync(int id);
        Task<HopDto> AddHopAsync(HopDto hopsDto);
        Task<HopDto> DeleteHopAsync(int id);
        Task UpdateHopAsync(HopDto hopDto);
        Task<IEnumerable<HopDto>> SearchHop(string query, int from, int size);
        Task ReIndexHopsElasticSearch();
        Task<IList<DTO>> GetHopFromsAsync();
        IEnumerable<DTO> GetHopFroms();
    }
}
