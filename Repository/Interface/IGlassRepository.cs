using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IGlassRepository
    {
        IList<Glass> GetAll(params string[] navigationProperties);
        Glass GetSingle(int id, params string[] navigationProperties);
        void Add(Glass glass);
        void Update(Glass glass);
        void Remove(Glass glass);

        //Async methods
        Task<IList<Glass>> GetAllAsync(params string[] navigationProperties);
        Task<Glass> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Glass glass);
        Task<int> UpdateAsync(Glass glass);
        Task RemoveAsync(Glass glass);
    }
}
