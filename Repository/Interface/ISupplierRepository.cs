using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface ISupplierRepository
    {
        IList<Supplier> GetAll(params string[] navigationProperties);
        Supplier GetSingle(int id, params string[] navigationProperties);
        void Add(Supplier supplier);
        void Update(Supplier supplier);
        void Remove(Supplier supplier);

        //Async methods
        Task<IList<Supplier>> GetAllAsync(params string[] navigationProperties);
        Task<Supplier> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Supplier supplier);
        Task<int> UpdateAsync(Supplier supplier);
        Task RemoveAsync(Supplier supplier);
    }
}
