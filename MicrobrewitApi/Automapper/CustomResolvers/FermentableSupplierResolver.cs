using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class FermentableSupplierResolver : ValueResolver<FermentablePostDto, int?>
    {
        protected override int? ResolveCore(FermentablePostDto source)
        {
            using (var context = new Microbrewit.Model.MicrobrewitContext())
            {
                Supplier supplier = null;
                if (source.Supplier != null)
                {
                    supplier = context.Suppliers.SingleOrDefault(s => s.Id == source.Supplier.Id || s.Name.Equals(source.Supplier.Name));

                    if (supplier == null)
                    {
                        supplier = new Supplier()
                        {
                            Name = source.Supplier.Name,
                        };
                        context.Suppliers.Add(supplier);
                        context.SaveChanges();
                    }
                return supplier.Id;
                }
                else
                {
                    return null;
                }
            }


        }
    }
}