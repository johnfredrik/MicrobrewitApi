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
    public interface ISupplierElasticsearch
    {
        Task UpdateAsync(SupplierDto supplierDto);

        Task<IEnumerable<SupplierDto>> GetAllAsync(string custom);
        Task<SupplierDto> GetSingleAsync(int id);
        Task<IEnumerable<SupplierDto>> SearchAsync(string query, int from, int size);
        Task UpdateAllAsync(IEnumerable<SupplierDto> supplierDtos);
        Task DeleteAsync(int id);
    }
}
