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
    public class IbuCalculationTests
    {
        [Test]
        public void CalculateTinseth()
        {
            var tinsethUtilisation = Formulas.TinsethUtilisation(1.05, 60);
            Assert.AreEqual(0.2307,Math.Round(tinsethUtilisation),4);
            var tinasethMgl = Formulas.TinsethMgl(50, 5, 20);
            var tinseth = Math.Round(Formulas.TinsethIbu(tinasethMgl, tinsethUtilisation),1);
            Assert.AreEqual(28.8,tinseth);

        }

        [Test]
        public void CalculateRanger()
        {
            var ragerUtilisation = Formulas.RangerUtilisation(60);
            var rager = Math.Round(Formulas.RangerIbu(50, ragerUtilisation, 5, 20, 1.05), 1);
            Assert.AreEqual(38.5, rager);

        }
    }
}
