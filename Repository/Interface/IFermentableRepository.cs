using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Repository
{
    public interface IFermentableRepository 
    {
        IList<Fermentable> GetAll(params string[] navigationProperties);
        Fermentable GetSingle(int id, params string[] navigationProperties);
        void Add(Fermentable fermentable);
        void Update(Fermentable fermentable);
        void Remove(Fermentable fermentable);

        //Async methods
        Task<IList<Fermentable>> GetAllAsync(params string[] navigationProperties);
        Task<Fermentable> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Fermentable fermentable);
        Task<int> UpdateAsync(Fermentable fermentable);
        Task RemoveAsync(Fermentable fermentable);
    }
}
