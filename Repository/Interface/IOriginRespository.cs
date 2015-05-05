using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IOriginRespository
    {
        IList<Origin> GetAll(params string[] navigationProperties);
        Origin GetSingle(int id, params string[] navigationProperties);
        void Add(params Origin[] items);
        void Update(params Origin[] items);
        void Remove(params Origin[] items);

        //Async methods
        Task<IList<Origin>> GetAllAsync(params string[] navigationProperties);
        Task<Origin> GetSingleAsync(Expression<Func<Origin, bool>> where, params string[] navigtionProperties);
        Task AddAsync(params Origin[] items);
        Task<int> UpdateAsync(params Origin[] items);
        Task RemoveAsync(params Origin[] items);
    }
}
