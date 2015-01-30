using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Nest;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IUserElasticsearch
    {
        Task<IIndexResponse> UpdateAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetSingleAsync(string username);
        Task<IEnumerable<UserDto>> SearchAsync(string query, int from, int size);
        Task<IBulkResponse> UpdateAllAsync(IEnumerable<UserDto> users);
        Task<IDeleteResponse> DeleteAsync(string username);
    }
}
