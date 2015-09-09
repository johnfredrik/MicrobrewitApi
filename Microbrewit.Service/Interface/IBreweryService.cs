using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IBreweryService
    {
        Task<IEnumerable<BreweryDto>> GetAllAsync(int from, int size);
        Task<BreweryDto> GetSingleAsync(int id);
        Task<BreweryDto> AddAsync(BreweryDto breweryDto);
        Task<BreweryDto> DeleteAsync(int id);
        Task UpdateAsync(BreweryDto breweryDto);
        Task<IEnumerable<BreweryDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
        Task ReIndexBeerRelationElasticSearch(BeerDto beerDto);
        Task ReIndexUserRelationElasticSearch(UserDto userDto);


        Task<BreweryMemberDto> GetBreweryMember(int breweryId, string username);
        Task<IEnumerable<BreweryMemberDto>> GetAllMembers(int breweryId);
        Task<BreweryMemberDto> DeleteMember(int breweryId, string username);
        Task UpdateBreweryMember(int breweryId,BreweryMemberDto breweryMember);
        Task<BreweryMemberDto> AddBreweryMember(int breweryId, BreweryMemberDto breweryMember);
        IEnumerable<BreweryMember> GetMemberships(string username);
    }
}
