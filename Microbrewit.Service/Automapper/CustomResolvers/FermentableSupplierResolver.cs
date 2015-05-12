using System.Linq;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class FermentableSupplierResolver : ValueResolver<FermentableDto, int?>
    {
        protected override int? ResolveCore(FermentableDto source)
        {
            using (var context = new MicrobrewitContext())
            {
                Supplier supplier = null;
                if (source.Supplier != null)
                {
                    supplier = context.Suppliers.SingleOrDefault(s => s.SupplierId == source.Supplier.Id || s.Name.Equals(source.Supplier.Name));

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
                else
                {
                    return null;
                }
            }


        }
    }
}