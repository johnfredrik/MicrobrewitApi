using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public interface IFermentableRepository
    {
        IList<Fermentable> GetFermentables();
        Fermentable GetFermentable(int fermentableId);

        IList<Grain> GetGrains();
        IList<Sugar> GetSugars();
        IList<DryExtract> GetDryExtracts();
        IList<LiquidExtract> GetLiquidExtracts();
    }
}
