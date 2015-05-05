using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class YeastSupplierResolver : ValueResolver<YeastDto, int?>
    {
        protected override int? ResolveCore(YeastDto source)
        {
            using (var context = new MicrobrewitContext())
            {
                if (source.Supplier != null)
                {
                    var supplier = context.Suppliers.SingleOrDefault(s => s.SupplierId == source.Supplier.Id || s.Name.Equals(source.Supplier.Name));

                    if (supplier == null)
                    {
                        supplier = new Supplier()
                        {
                            Name = source.Supplier.Name,
                        };
                        context.Suppliers.Add(supplier);
                        context.SaveChanges();
                    }
                    return supplier.SupplierId;
                }
                return null;
            }
        }
    }
}