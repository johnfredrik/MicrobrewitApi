using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IBeerRepository : IGenericDataRepository<Beer>
    {
        Task<IList<Beer>> GetLastAsync(int from, int size, params string[] navigationProperties);
        Task<IList<Beer>> GetAllUserBeerAsync(string username, params string[] navigationProperties);
        IList<Beer> GetAllUserBeer(string username, params string[] navigationProperties);
        IList<Beer> GetAllBreweryBeers(int breweryId, params string[] navigationProperties);

    }
}
