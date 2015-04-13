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
        IList<Hop> GetList(Expression<Func<Hop, bool>> where, params string[] navigationProperties);
        Hop GetSingle(Func<Hop, bool> where, params string[] navigationProperties);
        void Add(Hop item);
        void Update(Hop item);
        void Remove(Hop item);

        //Async methods
        Task<IList<Hop>> GetAllAsync(params string[] navigationProperties);
        Task<Hop> GetSingleAsync(Expression<Func<Hop, bool>> where, params string[] navigtionProperties);
        Task AddAsync(Hop item);
        Task<int> UpdateAsync(Hop item);
        Task RemoveAsync(Hop item);

        Flavour AddFlavour(string name);
        HopForm GetForm(Expression<Func<HopForm, bool>> where, params string[] navigationProperties);
        Task<IList<HopForm>> GetHopFormsAsync();
        IList<HopForm> GetHopForms();
    }
}
