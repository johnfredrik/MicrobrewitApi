using Microbrewit.Model;
using Microbrewit.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using StackExchange.Redis;
using Newtonsoft.Json;
using Microbrewit.Model.DTOs;
using System.Configuration;

namespace Microbrewit.Api.Util
{
    public static class Calculation
    {
        private static readonly string redisStore = ConfigurationManager.AppSettings["redis"];
        private static readonly ConnectionMultiplexer redis = ConnectionMultiplexer.Connect(redisStore);

        public static SRM CalculateSRM(Recipe recipe)
        {
            var srm = new SRM();

            foreach (var mashStep in recipe.MashSteps)
            {
                foreach (var fermentable in mashStep.Fermentables)
                {
                    srm.Standard += Math.Round(Formulas.MaltColourUnits(fermentable.Amount, fermentable.Lovibond, mashStep.Volume), 0);
                    srm.Morey += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, mashStep.Volume), 0);
                    srm.Mosher += Math.Round(Formulas.Morey(fermentable.Amount, fermentable.Lovibond, mashStep.Volume), 0);
                    srm.Daniels += Math.Round(Formulas.Daniels(fermentable.Amount, fermentable.Lovibond, mashStep.Volume), 0);
                }
            }

            return srm;
        }

        public static IBU CalculateIBU(Recipe recipe)
        {
            var og = recipe.OG;
            var ibu = new IBU();
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
            ibu.Rager = Math.Round(rager, 1);
            return ibu;
        }

        public static double CalculateOG(Recipe recipe)
        {
            var og = 0.0;
            foreach (var mashStep in recipe.MashSteps)
            {
                foreach (var fermentable in mashStep.Fermentables)
                {
                        var redisClient = redis.GetDatabase();
                        var redisFermentable = JsonConvert.DeserializeObject<FermentableDto>(redisClient.HashGet("fermentables", fermentable.FermentableId));
                        og += Formulas.MaltOG(fermentable.Amount, redisFermentable.PPG, recipe.Efficiency, recipe.Volume);
                }
            }
            return Math.Round(1 + og / 1000, 4);
        }

        public static ABV CalculateABV(Recipe recipe)
        {
            var abv = new ABV();
            abv.Standard = Math.Round(Formulas.MicrobrewitABV(recipe.OG, recipe.FG), 2);
            abv.Miller = Math.Round(Formulas.MillerABV(recipe.OG, recipe.FG), 2);
            abv.Simple = Math.Round(Formulas.SimpleABV(recipe.OG, recipe.FG), 2);
            abv.Advanced = Math.Round(Formulas.AdvancedABV(recipe.OG, recipe.FG), 2);
            abv.AdvancedAlternative = Math.Round(Formulas.AdvancedAlternativeABV(recipe.OG, recipe.FG), 2);
            abv.AlternativeSimple = Math.Round(Formulas.SimpleAlternativeABV(recipe.OG, recipe.FG), 2);

            return abv;
        }
    }
}