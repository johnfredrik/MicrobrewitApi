using System;
using Microbrewit.Api.Service.Util;
using Microbrewit.Api.Util;
using NUnit.Framework;

namespace Microbrewit.Test.Calculations
{
    [TestFixture]
    public class FormulasTests
    {
        [Test]
        public void CalculateMillerABV()
        {
            var miller = Math.Round(Formulas.MillerABV(1.05, 1.01),2);
            Assert.AreEqual(5.33, miller);
        }

        [Test]
        public void CalculateSimpleABV()
        {
            var simple = Math.Round(Formulas.SimpleABV(1.05, 1.01), 2);
            Assert.AreEqual(5.25,simple);
        }

        [Test]
        public void CalculateSimpleAlternativeAbv()
        {
            var abv = Math.Round(Formulas.SimpleAlternativeABV(1.05, 1.01), 2);
            Assert.AreEqual(5.26,abv);
        }

        [Test]
        public void CalculateAdvancedABV()
        {
            var advanced = Math.Round(Formulas.AdvancedABV(1.05, 1.01),2);
            Assert.AreEqual(5.19,advanced);     
        }

        [Test]
        public void CalculateAdvancedAlternativeAbv()
        {
            var abv = Math.Round(Formulas.AdvancedAlternativeABV(1.05, 1.01), 2);
            Assert.AreEqual(5.34,abv);
        }

        [Test]
        public void CalculateMicrobrewitABV()
        {
          
            //var simple = Math.Round(Formulas.SimpleAlternativeABV(1.05, 1.01), 2);
            //Assert.AreEqual(5.25,simple);
        }

        [Test]
        public void CalculateMaltOG()
        {
            var og = Formulas.MaltOG(6000, 35, 75, 20);
            og = Math.Round(1 + og / 1000, 3);
            Assert.AreEqual(1.066,og);
        }
    }
}
