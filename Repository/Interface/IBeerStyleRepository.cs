using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IBeerStyleRepository
    {
        IList<BeerStyle> GetAll(params string[] navigationProperties);
        BeerStyle GetSingle(int id, params string[] navigationProperties);
        void Add(BeerStyle beerStyle);
        void Update(BeerStyle beerStyle);
        void Remove(BeerStyle beerStyle);

        //Async methods
        Task<IList<BeerStyle>> GetAllAsync(params string[] navigationProperties);
        Task<BeerStyle> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(BeerStyle beerStyle);
        Task<int> UpdateAsync(BeerStyle beerStyle);
        Task RemoveAsync(BeerStyle beerStyle);
    }
}
