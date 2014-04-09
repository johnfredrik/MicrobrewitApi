using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Util
{
    public static class Formulas
    {
        /// <summary>
        /// Malt Color Units.
        /// The simplest equation for estimating the colour of beer.
        /// Good for beer colour < 10.5.
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="lovibond"></param>
        /// <param name="postBoilVolume"></param>
        /// <returns></returns>
        public static double MaltColourUnits(double weight, double lovibond, double postBoilVolume)
        {
            var volumeGallons = ConvertLitersToGallons(postBoilVolume);
            var grainWeight = ConvertGramsToPounds(weight);
            return (lovibond * grainWeight) / volumeGallons;
        }

        private static double ConvertLitersToGallons(double liters)
        {
            return liters * 0.26417;
        }

        private static double ConvertGramsToPounds(double grams)
        {
            return (grams / 1000) * 2.2046;
        }
        /// <summary>
        /// The Morey Equation.
        /// Good for beer color < 50 SRM
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="lovibond"></param>
        /// <param name="postBoilVolume"></param>
        /// <returns>SRM</returns>
        public static double Morey(double weight, double lovibond, double postBoilVolume)
        {
            return 1.4922 * Math.Pow(MaltColourUnits(weight, lovibond, postBoilVolume), 0.6859);
        }
        /// <summary>
        /// Daniels Equation
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="lovibond"></param>
        /// <param name="postBoilVolume"></param>
        /// <returns></returns>
        public static double Daniels(double weight, double lovibond, double postBoilVolume)
        {
            return (0.2 * MaltColourUnits(weight, lovibond, postBoilVolume)) + 8.4;
        }
        /// <summary>
        /// Mosher 
        /// </summary>
        /// <param name="weight"></param>
        /// <param name="lovibond"></param>
        /// <param name="postBoilVolume"></param>
        /// <returns></returns>
        public static double Mosher(double weight, double lovibond, double postBoilVolume) 
        {
			return (0.3 * MaltColourUnits(weight, lovibond, postBoilVolume)) + 4.7;
		}
        /// <summary>
        /// Converts SRM to Lovibond
        /// </summary>
        /// <param name="srm"></param>
        /// <returns></returns>
        public static double SRMToLovibond(double srm)
        {
            return (srm + 0.6) / 1.35;
        }
        /// <summary>
        /// Converts EBC to SRM
        /// </summary>
        /// <param name="ebc"></param>
        /// <returns></returns>
        public static double EBCToSRM(double ebc)
        {
            return ebc / 1.97;
        }

        /// <summary>
        /// Converts SRM to EBC
        /// </summary>
        /// <param name="srm"></param>
        /// <returns></returns>
        public static double SRMToEBC(double srm)
        {
            return srm * 1.97;
        }

    }
}