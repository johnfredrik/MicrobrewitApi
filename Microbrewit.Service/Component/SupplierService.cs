using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;

namespace Microbrewit.Service.Component
{
    public class SupplierService : ISupplierService
    {

        private ISupplierRepository _supplierRepository;
        private ISupplierElasticsearch _supplierElasticsearch;

        public SupplierService(ISupplierRepository supplierRepository, ISupplierElasticsearch supplierElasticsearch)
        {
            _supplierElasticsearch = supplierElasticsearch;
            _supplierRepository = supplierRepository;
        }
        public async Task<IEnumerable<SupplierDto>> GetAllAsync(string custom)
        {
            var supplierDtos = await _supplierElasticsearch.GetAllAsync(custom);
            if (supplierDtos != null) return supplierDtos;
            var suppliers = await _supplierRepository.GetAllAsync("Origin");
            supplierDtos = Mapper.Map<IEnumerable<Supplier>, IEnumerable<SupplierDto>>(suppliers);
            return supplierDtos;
        }

        public async Task<SupplierDto> GetSingleAsync(int id)
        {
            var supplierDto = await _supplierElasticsearch.GetSingleAsync(id);
            if (supplierDto != null) return supplierDto;
            var supplier = await _supplierRepository.GetSingleAsync(id, "Origin");
            supplierDto = Mapper.Map<Supplier, SupplierDto>(supplier);
            return supplierDto;
        }

        public async Task<SupplierDto> AddAsync(SupplierDto supplierDto)
        {
            var supplier = Mapper.Map<SupplierDto, Supplier>(supplierDto);
            await _supplierRepository.AddAsync(supplier);
            var result = await _supplierRepository.GetSingleAsync(supplier.Id, "Origin");
            var mappedResult = Mapper.Map<Supplier, SupplierDto>(result);
            await _supplierElasticsearch.UpdateAsync(mappedResult);
            return mappedResult;
        }

        public async Task<SupplierDto> DeleteAsync(int id)
        {
            var supplier = await _supplierRepository.GetSingleAsync(id);
            var supplierDto = await _supplierElasticsearch.GetSingleAsync(id);
            if (supplier != null) await _supplierRepository.RemoveAsync(supplier);
            if (supplierDto != null) await _supplierElasticsearch.DeleteAsync(id);
            return supplierDto;
        }

        public async Task UpdateAsync(SupplierDto supplierDto)
        {
            var supplier = Mapper.Map<SupplierDto, Supplier>(supplierDto);
            await _supplierRepository.UpdateAsync(supplier);
            var result = await _supplierRepository.GetSingleAsync(supplier.Id, "Origin");
            var mapperResult = Mapper.Map<Supplier, SupplierDto>(result);
            await _supplierElasticsearch.UpdateAsync(mapperResult);
        }

        public async Task<IEnumerable<SupplierDto>> SearchAsync(string query, int @from, int size)
        {
            return await _supplierElasticsearch.SearchAsync(query, from, size);
        }

        public async Task ReIndexElasticSearch()
        {
            var suppliers = await _supplierRepository.GetAllAsync("Origin");
            var supplierDtos = Mapper.Map<IEnumerable<Supplier>,IEnumerable<SupplierDto>>(suppliers);
            await _supplierElasticsearch.UpdateAllAsync(supplierDtos);
        }
    }
}
