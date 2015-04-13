using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Api.Service.Util;
using NUnit.Framework;

namespace Microbrewit.Test.Calculations
{
    [TestFixture]
    public class SrmCalculationTests
    {

        [Test]
        public void CalculateGramsToPounds()
        {
            var pounds = Math.Round(Formulas.ConvertGramsToPounds(1000),2);
            Assert.AreEqual(2.20,pounds);
        }

        [Test]
        public void CalculateLitersToGallons()
        {
            var gallons = Math.Round(Formulas.ConvertLitersToGallons(1), 2);
            Assert.AreEqual(0.26,gallons);
        }

        [Test]
        public void CalculateMaltColourUnits()
        {
            var mcu = Math.Round(Formulas.MaltColourUnits(1000, 20, 21),2);
            Assert.AreEqual(7.95,mcu);
        }

        [Test]
        public void CalculateMorey()
        {
            var morey = Math.Round(Formulas.Morey(1000, 20, 21),2);
            Assert.AreEqual(6.18,morey);
        }

        [Test]
        public void CalculateMosher()
        {
            var mosher = Math.Round(Formulas.Mosher(1000, 20, 21), 2);
            Assert.AreEqual(7.08, mosher);
        }

        [Test]
        public void CalculateDaniels()
        {
            var daniels = Math.Round(Formulas.Daniels(1000, 20, 21), 2);
            Assert.AreEqual(9.99,daniels);
        }
    }
}
