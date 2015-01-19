using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    class FermentableRepository
    {
        private static IFermentableRepository _fermentableRepository = new Microbrewit.Repository.FermentableRepository();

        [Test]
        public void GetAll()
        {
            var fermentables = _fermentableRepository.GetSingle(f => f.Id == 143,"Supplier.Origin","SubFermentables");

        }
    }
}
