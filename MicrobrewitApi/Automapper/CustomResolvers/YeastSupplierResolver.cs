using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model.DTOs;
using Microbrewit.Model;

namespace Microbrewit.Api.Automapper.CustomResolvers
{
    public class YeastSupplierResolver : ValueResolver<YeastPostDto, int>
    {
        protected override int ResolveCore(YeastPostDto source)
        {
            using (var context = new MicrobrewitContext())
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
                }

                return supplier.Id;
            }
        }
    }
}