using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IBeerRepository
    {
        IList<Beer> GetAll(int from, int size,params string[] navigationProperties);
        Beer GetSingle(int id, params string[] navigationProperties);
        void Add(Beer beer);
        void Update(Beer beer);
        void Remove(Beer beer);

        //Async methods
        Task<IList<Beer>> GetAllAsync(int from,int size,params string[] navigationProperties);
        Task<Beer> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Beer beer);
        Task<int> UpdateAsync(Beer beer);
        Task RemoveAsync(Beer beer);


        Task<IList<Beer>> GetLastAsync(int from, int size, params string[] navigationProperties);
        Task<IList<Beer>> GetAllUserBeerAsync(string username, params string[] navigationProperties);
        IList<Beer> GetAllUserBeer(string username, params string[] navigationProperties);
        IList<Beer> GetAllBreweryBeers(int breweryId, params string[] navigationProperties);

    }
}
