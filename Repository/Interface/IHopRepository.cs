using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IHopRepository : IGenericDataRepository<Hop>
    {
        Flavour AddFlavour(string name);
        HopForm GetForm(Expression<Func<HopForm, bool>> where, params string[] navigationProperties);
        Task<IList<HopForm>> GetHopFormsAsync();
    }
}
