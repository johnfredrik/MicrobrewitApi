using System;
using System.Linq;
using Microbrewit.Api.Service.Util;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Service.Util
{
    public static class Calculation
    {
        private static IFermentableElasticsearch _fermentableElasticsearch = new FermentableElasticsearch();
        private static IFermentableRepository _fermentableRepository = new FermentableDapperRepository();

        public static SRM CalculateSRM(Recipe recipe)
        {
            var srm = new SRM{SrmId = recipe.RecipeId};
            foreach (var mashStep in recipe.MashSteps)
            {
                var volume = recipe.Volume;
                if (mashStep.Volume > 0)
                    volume = mashStep.Volume;
                foreach (var fermentable in mashStep.Fermentables)
                {
                    srm.Standard += Math.Round(Formulas.MaltColourUnits(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Morey += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Mosher += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Daniels += Math.Round(Formulas.Daniels(fermentable.Amount, fermentable.Lovibond, volume), 0);
                }
            }

            return srm;
        }

        public static SRMDto CalculateSRMDto(RecipeDto recipe)
        {
            var srm = new SRMDto { SrmId = recipe.Id };
            foreach (var mashStep in recipe.MashSteps)
            {
                var volume = recipe.Volume;
                if (mashStep.Volume > 0)
                    volume = mashStep.Volume;
                foreach (var fermentable in mashStep.Fermentables.Where(f => f != null))
                {
                    srm.Standard += Math.Round(Formulas.MaltColourUnits(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Morey += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Mosher += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, volume), 0);
                    srm.Daniels += Math.Round(Formulas.Daniels(fermentable.Amount, fermentable.Lovibond, volume), 0);
                }
            }

            return srm;
        }

        public static IBU CalculateIBU(Recipe recipe)
        {
            var og = recipe.OG;
            var ibu = new IBU {IbuId = recipe.RecipeId};
           
            var tinseth = 0.0;
            var rager = 0.0;
            foreach (var boilStep in recipe.BoilSteps)
            {
                var tinsethUtilisation = Formulas.TinsethUtilisation(og, boilStep.Length);
                var ragerUtilisation = Formulas.RangerUtilisation(boilStep.Length);
                foreach (var hop in boilStep.Hops)
                {
                    var tinasethMgl = Formulas.TinsethMgl(hop.Amount, hop.AAValue, recipe.Volume);
                    tinseth += Formulas.TinsethIbu(tinasethMgl, tinsethUtilisation);
                    rager += Formulas.RangerIbu(hop.Amount, ragerUtilisation, hop.AAValue, recipe.Volume, og);
                }
            }
            
            ibu.Tinseth = Math.Round(tinseth, 1);
            ibu.Standard = Math.Round(tinseth, 1);
            ibu.Rager = Math.Round(rager, 1);
            return ibu;
        }

        public static IBUDto CalculateIBUDto(RecipeDto recipe)
        {
            var og = recipe.OG;
            var ibu = new IBUDto { IbuId = recipe.Id };

            var tinseth = 0.0;
            var rager = 0.0;
            foreach (var boilStep in recipe.BoilSteps)
            {
                var tinsethUtilisation = Formulas.TinsethUtilisation(og, boilStep.Length);
                var ragerUtilisation = Formulas.RangerUtilisation(boilStep.Length);
                foreach (var hop in boilStep.Hops)
                {
                    var tinasethMgl = Formulas.TinsethMgl(hop.Amount, hop.AAValue, recipe.Volume);
                    tinseth += Formulas.TinsethIbu(tinasethMgl, tinsethUtilisation);
                    rager += Formulas.RangerIbu(hop.Amount, ragerUtilisation, hop.AAValue, recipe.Volume, og);
                }
            }

            ibu.Tinseth = Math.Round(tinseth, 1);
            ibu.Standard = Math.Round(tinseth, 1);
            ibu.Rager = Math.Round(rager, 1);
            return ibu;
        }

        public static double CalculateOG(Recipe recipe)
        {
            var og = 0.0;

            foreach (var fermentable in recipe.MashSteps.SelectMany(mashStep => mashStep.Fermentables))
            {
                var efficency = recipe.Efficiency;
                //if (fermentable.PPG <= 0)
                //{
                    var esFermentable = _fermentableElasticsearch.GetSingle(fermentable.FermentableId);
                    if (esFermentable != null && esFermentable.PPG > 0)
                    {
                        fermentable.PPG = esFermentable.PPG;
                        if (esFermentable.Type.Contains("Extract") || esFermentable.Type.Contains("Sugar"))
                            efficency = 100;
                        //og += Formulas.MaltOG(fermentable.Amount, esFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }
                    else
                    {
                        var efFermentable = _fermentableRepository.GetSingle(fermentable.FermentableId);
                        if (efFermentable != null && efFermentable.PPG != null)
                        {
                            fermentable.PPG = (int)efFermentable.PPG;
                            if (efFermentable.Type.Contains("Extract") || efFermentable.Type.Contains("Sugar"))
                                efficency = 100;
                        }
                        //og += Formulas.MaltOG(fermentable.Amount, (int)efFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }

                //}
                og += Formulas.MaltOG(fermentable.Amount, (int)fermentable.PPG, efficency, recipe.Volume);
            }
            return Math.Round(1 + og / 1000, 4);
        }

        public static double CalculateOGDto(RecipeDto recipe)
        {
            var og = 0.0;

            try
            {
                foreach (var fermentable in recipe.MashSteps.SelectMany(mashStep => mashStep.Fermentables.Where(f => f != null)))
                {
                    var efficency = recipe.Efficiency;
                    //if (fermentable.PPG <= 0)
                    //{
                    var esFermentable = _fermentableElasticsearch.GetSingle(fermentable.FermentableId);
                    if (esFermentable != null && esFermentable.PPG > 0)
                    {
                        fermentable.PPG = esFermentable.PPG;
                        if (esFermentable.Type.Contains("Extract") || esFermentable.Type.Contains("Sugar"))
                            efficency = 100;
                        //og += Formulas.MaltOG(fermentable.Amount, esFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }
                    else
                    {
                        var efFermentable = _fermentableRepository.GetSingle(fermentable.FermentableId);
                        if (efFermentable?.PPG != null)
                        {
                            fermentable.PPG = (int) efFermentable.PPG;
                            if (efFermentable.Type.Contains("Extract") || efFermentable.Type.Contains("Sugar"))
                                efficency = 100;
                        }
                        //og += Formulas.MaltOG(fermentable.Amount, (int)efFermentable.PPG, recipe.Efficiency, recipe.Volume);
                    }

                    //}
                    og += Formulas.MaltOG(fermentable.Amount, (int) fermentable.PPG, efficency, recipe.Volume);
                }
            }
            catch (Exception exception)
            {

                throw;
            }

            return Math.Round(1 + og / 1000, 4);
        }


        public static ABV CalculateABV(Recipe recipe)
        {
            var abv = new ABV
            {
                AbvId = recipe.RecipeId,
                Miller = Math.Round(Formulas.MillerABV(recipe.OG, recipe.FG), 2),
                Simple = Math.Round(Formulas.SimpleABV(recipe.OG, recipe.FG), 2),
                Advanced = Math.Round(Formulas.AdvancedABV(recipe.OG, recipe.FG), 2),
                AdvancedAlternative = Math.Round(Formulas.AdvancedAlternativeABV(recipe.OG, recipe.FG), 2),
                AlternativeSimple = Math.Round(Formulas.SimpleAlternativeABV(recipe.OG, recipe.FG), 2),
                Standard = Math.Round(Formulas.MicrobrewitABV(recipe.OG, recipe.FG), 2)
            };

            return abv;
        }

        public static ABVDto CalculateABVDto(RecipeDto recipe)
        {
            var abv = new ABVDto
            {
                AbvId = recipe.Id,
                Miller = Math.Round(Formulas.MillerABV(recipe.OG, recipe.FG), 2),
                Simple = Math.Round(Formulas.SimpleABV(recipe.OG, recipe.FG), 2),
                Advanced = Math.Round(Formulas.AdvancedABV(recipe.OG, recipe.FG), 2),
                AdvancedAlternative = Math.Round(Formulas.AdvancedAlternativeABV(recipe.OG, recipe.FG), 2),
                AlternativeSimple = Math.Round(Formulas.SimpleAlternativeABV(recipe.OG, recipe.FG), 2),
                Standard = Math.Round(Formulas.MicrobrewitABV(recipe.OG, recipe.FG), 2)
            };

            return abv;
        }
    }
}