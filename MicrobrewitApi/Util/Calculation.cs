using Microbrewit.Model;
using Microbrewit.Api.Util;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Util
{
    public static class Calculation
    {
        public static SRM CalculateSRM(Recipe recipe)
        {
            var srm = new SRM();
            
            foreach (var mashStep in recipe.MashSteps)
            {
                foreach (var fermentable in mashStep.Fermentables)
                {
                    srm.Standard += Formulas.MaltColourUnits(fermentable.Amount, fermentable.Lovibond, mashStep.Volume);
                    srm.Morey += Formulas.Morey(fermentable.Amount, fermentable.Lovibond, mashStep.Volume);
                    srm.Mosher += Formulas.Morey(fermentable.Amount, fermentable.Lovibond, mashStep.Volume);
                    srm.Daniels += Formulas.Daniels(fermentable.Amount, fermentable.Lovibond, mashStep.Volume);
                }
            }

            return srm;
        }
    }
}