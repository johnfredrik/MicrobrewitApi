using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Api.Areas.HelpPage;
using NUnit.Framework;
using Microbrewit.Api.Util;

namespace Microbrewit.Test.Calculations
{
    [TestFixture]
    public class AbvCalculationTests
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
        public void CalculateAdvancedABV()
        {
            var advanced = Math.Round(Formulas.AdvancedABV(1.05, 1.01),2);
            Assert.AreEqual(5.19,advanced);     
        }


    }
}
