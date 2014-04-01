using Microbrewit.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class YeastRepository : GenericDataRepository<Yeast>, IYeastRepository 
    {

        public Yeast AddYeast(Model.DTOs.YeastPostDto yeastPost)
        {
            using (var context = new MicrobrewitContext())
            {
                int yeastId;
                if(yeastPost.Type.Equals("Liquid Yeast"))
                {
                    var yeast = new LiquidYeast();
                    yeast.Name = yeastPost.Name;
                    if (yeastPost.ProductCode != null)
                    {
                        yeast.ProductCode = yeastPost.ProductCode;
                    }
                    if (yeastPost.TemperatureLow > 0)
                    {
                        yeast.TemperatureLow = yeastPost.TemperatureLow;
                    }
                    if (yeastPost.TemperatureHigh > 0)
                    {
                        yeast.TemperatureHigh = yeastPost.TemperatureHigh;
                    }
                    if (yeastPost.AlcoholTolerance != null)
                    {
                        yeast.AlcoholTolerance = yeastPost.AlcoholTolerance;
                    }
                    if (yeastPost.Comment != null)
                    {
                        yeast.Comment = yeastPost.Comment;
                    }
                    if(yeastPost.Flocculation != null)
                    {
                        yeast.Flocculation = yeastPost.Flocculation;
                    }
                    if (yeastPost.Supplier.Id > 0)
                    {
                        var supplier = context.Suppliers.SingleOrDefault(s => s.Id == yeastPost.Supplier.Id);
                        if (supplier != null)
                        {
                            yeast.SupplierId = supplier.Id;
                        }
                    }
                    else if (yeastPost.Supplier.Name != null)
                    {
                        var supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(yeastPost.Supplier.Name));
                        if (supplier != null)
                        {
                            yeast.SupplierId = supplier.Id;
                        }

                    }
                    context.Yeasts.Add(yeast);
                    context.SaveChanges();
                    yeastId = yeast.Id;
                }
                else if (yeastPost.Type.Equals("Dry Yeast"))
                {
                    var yeast = new DryYeast();
                    yeast.Name = yeastPost.Name;
                    if (yeastPost.ProductCode != null)
                    {
                        yeast.ProductCode = yeastPost.ProductCode;
                    }
                    if (yeastPost.TemperatureLow > 0)
                    {
                        yeast.TemperatureLow = yeastPost.TemperatureLow;
                    }
                    if (yeastPost.TemperatureHigh > 0)
                    {
                        yeast.TemperatureHigh = yeastPost.TemperatureHigh;
                    }
                    if (yeastPost.AlcoholTolerance != null)
                    {
                        yeast.AlcoholTolerance = yeastPost.AlcoholTolerance;
                    }
                    if (yeastPost.Comment != null)
                    {
                        yeast.Comment = yeastPost.Comment;
                    }
                    if (yeastPost.Flocculation != null)
                    {
                        yeast.Flocculation = yeastPost.Flocculation;
                    }
                    if (yeastPost.Supplier.Id > 0)
                    {
                        var supplier = context.Suppliers.SingleOrDefault(s => s.Id == yeastPost.Supplier.Id);
                        if (supplier != null)
                        {
                            yeast.SupplierId = supplier.Id;
                        }
                    }
                    else if (yeastPost.Supplier.Name != null)
                    {
                        var supplier = context.Suppliers.SingleOrDefault(s => s.Name.Equals(yeastPost.Supplier.Name));
                        if (supplier != null)
                        {
                            yeast.SupplierId = supplier.Id;
                        }

                    }

                    context.Yeasts.Add(yeast);
                    context.SaveChanges();
                    yeastId = yeast.Id;
                }
                else
                {
                    var yeast = new Yeast();
                    yeast.Name = yeastPost.Name;
                    if (yeastPost.ProductCode != null)
                    {
                        yeast.ProductCode = yeastPost.ProductCode;
                    }
                    if (yeastPost.TemperatureLow > 0)
                    {
                        yeast.TemperatureLow = yeastPost.TemperatureLow;
                    }
                    if (yeastPost.TemperatureHigh > 0)
                    {
                        yeast.TemperatureHigh = yeastPost.TemperatureHigh;
                    }
                    if (yeastPost.AlcoholTolerance != null)
                    {
                        yeast.AlcoholTolerance = yeastPost.AlcoholTolerance;
                    }
                    if (yeastPost.Comment != null)
                    {
                        yeast.Comment = yeastPost.Comment;
                    }
                    if (yeastPost.Flocculation != null)
                    {
                        yeast.Flocculation = yeastPost.Flocculation;
                    }

                    context.Yeasts.Add(yeast);
                    context.SaveChanges();
                    yeastId = yeast.Id;
                }
                return context.Yeasts.Find(yeastId);
            }
        }

      
    }
}
