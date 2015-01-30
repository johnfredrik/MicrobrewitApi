using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface ISupplierService
    {
        Task<IEnumerable<SupplierDto>> GetAllAsync(string custom);
        Task<SupplierDto> GetSingleAsync(int id);
        Task<SupplierDto> AddAsync(SupplierDto supplierDto);
        Task<SupplierDto> DeleteAsync(int id);
        Task UpdateAsync(SupplierDto supplierDto);
        Task<IEnumerable<SupplierDto>> SearchAsync(string query, int from, int size);
        Task ReIndexElasticSearch();
    }
}
