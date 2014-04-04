using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public class SupplierRepository : GenericDataRepository<Supplier>, ISupplierRepository
    {
        public override void Add(params Supplier[] suppliers)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var supplier in suppliers)
                {
                    if (supplier.Origin != null)
                    {
                        supplier.Origin = null;
                    }
                }
                base.Add(suppliers);
            }
        }
    }
}
