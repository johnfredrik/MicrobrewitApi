using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.Service.Util
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

        public static double ConvertLitersToGallons(double liters)
        {
            return liters * 0.26417;
        }

        public static double ConvertGramsToPounds(double grams)
        {
            return (grams / 1000) * 2.2046;
        }

        public static double FahrenheitToCelsius(double fahrenheit)
        {
            return (fahrenheit - 32)/2;
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

        public static double TinsethUtilisation(double og, int boilTime)
        {
            var boiltimeFactor = (1 - Math.Exp(-0.04 * boilTime)) / 4.15;
            var bignessFactor = 1.65 * Math.Pow(0.000125, (og - 1));
            return bignessFactor * boiltimeFactor;
        }

        public static double TinsethMgl(double weight, double alphaAcid, double batchSize)
        {
            alphaAcid = alphaAcid / 100;
            return (alphaAcid * weight * 1000) / batchSize;
        }

        public static double TinsethIbu(double mgl, double utilisation)
        {
            return utilisation * mgl;
        }

        private static double Tanh(double tanh)
        {
            var e = Math.Exp(2 * tanh);
            return (e - 1) / (e + 1);
        }

        public static double RangerUtilisation(int boilTime)
        {
            return (18.11 + 13.86 * Math.Tanh((boilTime - 31.32)/18.27))/100;
        }

        public static double RangerIbu(double weight, double utilisation, double alphaAcid, int boilVolume, double boilGravity)
        {
            double ga = 0.0;
            alphaAcid = alphaAcid / 100;
            if (boilGravity > 1.050)
            {
                ga = (boilGravity - 1.050) / 0.2;
            }
            return (weight * utilisation * alphaAcid * 1000) / (boilVolume * (1 + ga));
        }

        public static double MaltOG(double weight, int ppg, double efficiency, int volume)
        {
            return ((ConvertGramsToPounds(weight) * ppg * (efficiency / 100)) / ConvertLitersToGallons(volume));
        }
        /// <summary>
        /// Dave Miller formula of 1988
        /// </summary>
        /// <param name="og"></param>
        /// <param name="fg"></param>
        /// <returns></returns>
        public static double MillerABV(double og, double fg)
        {
            return ((og - fg) / 0.75) * 100;
        }
        /// <summary>
        /// Simplified "rule of thumb".
        /// </summary>
        /// <param name="og"></param>
        /// <param name="fg"></param>
        /// <returns></returns>
        public static double SimpleABV(double og, double fg)
        {
            return (og - fg) * 131.25;
        }
        /// <summary>
        /// Advanced simple
        /// </summary>
        /// <param name="og"></param>
        /// <param name="fg"></param>
        /// <returns></returns>
        public static double AdvancedABV(double og, double fg)
        {
            return ((og - fg) * (100.3*(og - fg) + 125.65));
        }

        /// <summary>
        ///  rumored to be more accurate at high gravities.
        /// </summary>
        /// <param name="og"></param>
        /// <param name="fg"></param>
        /// <returns></returns>
        public static double AdvancedAlternativeABV(double og, double fg)
        {
            return (76.08 * (og-fg) / (1.775-og)) * (fg / 0.794);
        }

        /// <summary>
        /// Another formula.
        /// </summary>
        /// <param name="og"></param>
        /// <param name="fg"></param>
        /// <returns></returns>
        public static double SimpleAlternativeABV(double og, double fg)
        {
            return ((1.05 / 0.79) * ((og - fg)/ fg) * 100);
        }

        public static double MicrobrewitABV(double og, double fg)
        {
            return ((SimpleAlternativeABV(og, fg) + AdvancedAlternativeABV(og, fg) + AdvancedABV(og, fg) + SimpleABV(og, fg) + MillerABV(og, fg)) / 5);
        }

        public static double ConvertPoundsToGrams(double pounds)
        {
            return Math.Round(pounds/0.0022046,0);
        }

        public static double ConvertOuncesToGrams(double ounces)
        {
            return Math.Round(ounces/0.035274, 0);
        }
    }
}