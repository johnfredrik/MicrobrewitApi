using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper.Internal;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Elasticsearch.Interface
{
    public interface IFermentableElasticsearch
    {
        Task UpdateAsync(FermentableDto fermentableDto);
        Task<IEnumerable<FermentableDto>> GetAllAsync(string custom);
        Task<FermentableDto> GetSingleAsync(int id);
        Task<IEnumerable<FermentableDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<FermentableDto> fermentableDtos);
        Task DeleteAsync(int id);

        FermentableDto GetSingle(int id);
        IEnumerable<FermentableDto> GetAll(string custom);
        IEnumerable<FermentableDto> Search(string query, int from, int size);
    }
}
