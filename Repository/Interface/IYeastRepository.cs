using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IYeastRepository
    {
        IList<Yeast> GetAll(params string[] navigationProperties);
        Yeast GetSingle(int id, params string[] navigationProperties);
        void Add(Yeast yeast);
        void Update(Yeast yeast);
        void Remove(Yeast yeast);

        //Async methods
        Task<IList<Yeast>> GetAllAsync(params string[] navigationProperties);
        Task<Yeast> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Yeast yeast);
        Task<int> UpdateAsync(Yeast yeast);
        Task RemoveAsync(Yeast yeast);
    }
}
