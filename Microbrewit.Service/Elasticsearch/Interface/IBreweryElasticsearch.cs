using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IBreweryElasticsearch
    {
        Task UpdateAsync(BreweryDto breweryDto);

        Task<IEnumerable<BreweryDto>> GetAllAsync();
        Task<BreweryDto> GetSingleAsync(int id);
        Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<BreweryDto> breweryDtos);
        Task DeleteAsync(int id);
        Task<IEnumerable<BreweryMemberDto>> GetAllMembersAsync(int breweryId);
        Task<BreweryMemberDto> GetSingleMemberAsync(int breweryId, string username);
        IEnumerable<BreweryMemberDto> GetMemberships(string username);
    }
}
