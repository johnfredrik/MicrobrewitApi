using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IHopRepository
    {
        IList<Hop> GetAll(params string[] navigationProperties);
        Hop GetSingle(int id, params string[] navigationProperties);
        void Add(Hop item);
        void Update(Hop item);
        void Remove(Hop hop);

        //Async methods
        Task<IList<Hop>> GetAllAsync(params string[] navigationProperties);
        Task<Hop> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Hop item);
        Task<int> UpdateAsync(Hop item);
        Task RemoveAsync(Hop item);

        Flavour AddFlavour(string name);
        HopForm GetForm(int id);
        Task<IList<HopForm>> GetHopFormsAsync();
        IList<HopForm> GetHopForms();
    }
}
