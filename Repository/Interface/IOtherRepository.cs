using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IOtherRepository
    {
        IList<Other> GetAll(params string[] navigationProperties);
        Other GetSingle(int id, params string[] navigationProperties);
        void Add(Other other);
        void Update(Other other);
        void Remove(Other other);

        //Async methods
        Task<IList<Other>> GetAllAsync(params string[] navigationProperties);
        Task<Other> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Other origin);
        Task<int> UpdateAsync(Other origin);
        Task RemoveAsync(Other origin);
    }
}
