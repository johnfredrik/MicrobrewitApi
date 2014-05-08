using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IGenericDataRepository<T> where T : class
    {
        IList<T> GetAll(params string[] navigationProperties);
        IList<T> GetList(Expression<Func<T, bool>> where, params string[] navigationProperties);
        T GetSingle(Func<T, bool> where, params string[] navigationProperties);
        void Add(params T[] items);
        void Update(params T[] items);
        void Remove(params T[] items);

        //Async methods
        Task<IList<T>> GetAllAsync(params string[] navigationProperties);
        Task<T> GetSingleAsync(Expression<Func<T, bool>> where, params string[] navigtionProperties);
        Task<int> AddAsync(params T[] items);
        Task<int> UpdateAsync(params T[] items);
        Task<int> RemoveAsync(params T[] items);
    }
}
