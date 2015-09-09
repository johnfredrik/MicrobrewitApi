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
        void Add(Origin origin);
        void Update(Origin origin);
        void Remove(Origin origin);

        //Async methods
        Task<IList<Origin>> GetAllAsync(int from, int size, params string[] navigationProperties);
        Task<Origin> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Origin origin);
        Task<int> UpdateAsync(Origin origin);
        Task RemoveAsync(Origin origin);
    }
}
