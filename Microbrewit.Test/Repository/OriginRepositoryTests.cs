using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class OriginRepositoryTests
    {

        private IOriginRespository _originRespository;

        [TestFixtureSetUp]
        public void TestFixureSetUp()
        {
            _originRespository = new OriginDapperRepository();
        }

        [Test]
        public void GetAll_Return_Not_Null_Not_Empty()
        {
            var origins = _originRespository.GetAll();
            Assert.NotNull(origins);
            Assert.True(origins.Any());
        }

        [Test]
        public void GetSingle_Return_Not_Null_Not_Empty()
        {
            var origin = _originRespository.GetSingle(1);
            Assert.NotNull(origin);
            Assert.True(origin.Name.Any());
        }

        [Test]
        public void Add_Gets_Added()
        {
            var origin = new Origin {Name = "Ankh Morpork"};
            _originRespository.Add(origin);
            var origins = _originRespository.GetAll();
            Assert.True(origins.Any(o => o.Name == origin.Name));
        }

        [Test]
        public void Add_OriginId_Gets_Set()
        {
            var newOrigin = new Origin { Name = "Ankh Morpork" };
            _originRespository.Add(newOrigin);
            var origin = _originRespository.GetSingle(newOrigin.OriginId);
            Assert.NotNull(origin);
        }

        [Test]
        public void Update_Gets_Updated()
        {
            var origin = _originRespository.GetAll().LastOrDefault();
            origin.Name = "Pseudopolis";
            _originRespository.Update(origin);
            var origins = _originRespository.GetAll();
            Assert.True(origins.Any(o => o.Name == origin.Name));
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newOrigin = new Origin { Name = "Ankh Morpork" };
            _originRespository.Add(newOrigin);
            _originRespository.Remove(newOrigin);
            var origins = _originRespository.GetAll();
            Assert.True(origins.All(o => o.OriginId != newOrigin.OriginId));
        }
    }
}
