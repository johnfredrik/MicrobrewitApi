using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;


namespace Microbrewit.Repository
{
    public class FermentableRepository : GenericDataRepository<Fermentable>, IFermentableRepository
    {

        //public IList<Fermentable> GetFermentables()
        //{
        //    using (var context = new MicrobrewitContext())
        //    {
        //        return context.Fermentables
        //            .Include("Supplier")
        //            .ToList();
        //    }
        //}

        //public Fermentable GetFermentable(int fermentableId)
        //{
        //    using (var context = new MicrobrewitContext())
        //    {
        //        return context.Fermentables
        //            .Include("Supplier.Origin")
        //            .Where(f => f.Id == fermentableId)
        //            .SingleOrDefault();
        //    }
        //}


        //public IList<Grain> GetGrains()
        //{
        //    using (var context = new MicrobrewitContext())
        //    {
        //        return context.Fermentables
        //            .Include("Supplier.Origin")
        //            .OfType<Grain>()
        //            .ToList();
        //    }
        //}

        //public IList<Sugar> GetSugars()
        //{
        //    using (var context = new MicrobrewitContext())
        //    {
        //        return context.Fermentables
        //                            .Include("Supplier.Origin")
        //                            .OfType<Sugar>()
        //                            .ToList();
        //    }
        //}

        //public IList<DryExtract> GetDryExtracts()
        //{
        //    using (var context = new MicrobrewitContext())
        //    {
        //        return context.Fermentables
        //            .Include("Supplier.Origin")
        //            .OfType<DryExtract>()
        //            .ToList();
        //    }
        //}

        //public IList<LiquidExtract> GetLiquidExtracts()
        //{
        //   using(var context = new MicrobrewitContext())
        //   {

        //        return context.Fermentables
        //                .Include("Supplier.Origin")
        //                .OfType<LiquidExtract>()
        //                .ToList();
        //    }
        //}

        public Fermentable AddFermentable(FermentablePostDto fermentablePost)
        {
            using (var context = new MicrobrewitContext())
            {
                int fermentableId;

                if (fermentablePost.Type.Equals("Grain"))
                {
                    var grain = new Grain();
                    Supplier supplier;
                    grain.Name = fermentablePost.Name;
                    if (fermentablePost.PPG != null)
                    {
                        grain.PPG = fermentablePost.PPG;
                    }
                    if (fermentablePost.EBC != null)
                    {
                        grain.EBC = fermentablePost.EBC;
                    }
                    if (fermentablePost.Supplier.Id > 0)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Id == fermentablePost.Supplier.Id);
                        if (supplier != null)
                        {
                            grain.SupplierId = supplier.Id;
                        }
                    }
                    else if(fermentablePost.Supplier.Name != null)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(fermentablePost.Supplier.Name));
                        if (supplier != null)
                        {
                            grain.SupplierId = supplier.Id;
                        }
                        
                    }

                    context.Fermentables.Add(grain);
                    context.SaveChanges();
                    fermentableId = grain.Id;


                }
                else if (fermentablePost.Type.Equals("Sugar"))
                {
                    var sugar = new Sugar();
                    Supplier supplier;
                    sugar.Name = fermentablePost.Name;
                    if (fermentablePost.PPG != null)
                    {
                        sugar.PPG = fermentablePost.PPG;
                    }
                    if (fermentablePost.EBC != null)
                    {
                        sugar.EBC = fermentablePost.EBC;
                    }
                    if (fermentablePost.Supplier.Id > 0)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Id == fermentablePost.Supplier.Id);
                        if (supplier != null)
                        {
                            sugar.SupplierId = supplier.Id;
                        }
                    }
                    else if (fermentablePost.Supplier.Name != null)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(fermentablePost.Supplier.Name));
                        if (supplier != null)
                        {
                            sugar.SupplierId = supplier.Id;
                        }

                    }

                    context.Fermentables.Add(sugar);
                    context.SaveChanges();
                     fermentableId = sugar.Id;
                }
                else if (fermentablePost.Type.Equals("Liquid Extract"))
                {
                    var liquidExtract = new LiquidExtract();
                    Supplier supplier;
                    liquidExtract.Name = fermentablePost.Name;
                    if (fermentablePost.PPG != null)
                    {
                        liquidExtract.PPG = fermentablePost.PPG;
                    }
                    if (fermentablePost.EBC != null)
                    {
                        liquidExtract.EBC = fermentablePost.EBC;
                    }
                    if (fermentablePost.Supplier.Id > 0)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Id == fermentablePost.Supplier.Id);
                        if (supplier != null)
                        {
                            liquidExtract.SupplierId = supplier.Id;
                        }
                    }
                    else if (fermentablePost.Supplier.Name != null)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(fermentablePost.Supplier.Name));
                        if (supplier != null)
                        {
                            liquidExtract.SupplierId = supplier.Id;
                        }

                    }

                    context.Fermentables.Add(liquidExtract);
                    context.SaveChanges();
                     fermentableId = liquidExtract.Id;
                }
                else if (fermentablePost.Type.Equals("Dry Extract"))
                {
                    var dryExtract = new DryExtract();
                    Supplier supplier;
                    dryExtract.Name = fermentablePost.Name;
                    if (fermentablePost.PPG != null)
                    {
                        dryExtract.PPG = fermentablePost.PPG;
                    }
                    if (fermentablePost.EBC != null)
                    {
                        dryExtract.EBC = fermentablePost.EBC;
                    }
                    if (fermentablePost.Supplier.Id > 0)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Id == fermentablePost.Supplier.Id);
                        if (supplier != null)
                        {
                            dryExtract.SupplierId = supplier.Id;
                        }
                    }
                    else if (fermentablePost.Supplier.Name != null)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(fermentablePost.Supplier.Name));
                        if (supplier != null)
                        {
                            dryExtract.SupplierId = supplier.Id;
                        }

                    }

                    context.Fermentables.Add(dryExtract);
                    context.SaveChanges();
                     fermentableId = dryExtract.Id;
                }
                else
                {
                    var fermentable = new Fermentable();
                    Supplier supplier;
                    fermentable.Name = fermentablePost.Name;
                    if (fermentablePost.PPG != null)
                    {
                        fermentable.PPG = fermentablePost.PPG;
                    }
                    if (fermentablePost.EBC != null)
                    {
                        fermentable.EBC = fermentablePost.EBC;
                    }
                    if (fermentablePost.Supplier.Id > 0)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Id == fermentablePost.Supplier.Id);
                        if (supplier != null)
                        {
                            fermentable.SupplierId = supplier.Id;
                        }
                    }
                    else if (fermentablePost.Supplier.Name != null)
                    {
                        supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(fermentablePost.Supplier.Name));
                        if (supplier != null)
                        {
                            fermentable.SupplierId = supplier.Id;
                        }

                    }

                    context.Fermentables.Add(fermentable);
                    context.SaveChanges();
                    fermentableId = fermentable.Id;

                }

                return context.Fermentables.Include("Supplier").SingleOrDefault(f => f.Id == fermentableId);

            }

        }

    }
}
