using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class OtherRepositoryTests
    {
        private IOtherRepository _otherRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _otherRepository = new OtherDapperRepository();
        }

        [Test]
        public void GetAll_Not_Null_Not_Empty()
        {
            var others = _otherRepository.GetAll();
            Assert.NotNull(others);
            Assert.True(others.Any());
        }

        [Test]
        public async Task GetAllAsync_Not_Null_Not_Empty()
        {
            var others = await _otherRepository.GetAllAsync();
            Assert.NotNull(others);
            Assert.True(others.Any());
        }

        [Test]
        public void GetSingle_Not_Null_Not_Empty()
        {
            var other = _otherRepository.GetSingle(1);
            Assert.NotNull(other);
            Assert.True(other.Name.Any());
        }

        [Test]
        public async Task GetSingleAsync_Not_Null_Not_Empty()
        {
            var other = await _otherRepository.GetSingleAsync(1);
            Assert.NotNull(other);
            Assert.True(other.Name.Any());
        }

        [Test]
        public void Add_Gets_Added()
        {
            var newOther = new Other {Name = "newOther" + DateTime.Now.Ticks, Type = "Something", Custom = true};
            _otherRepository.Add(newOther);
            var others = _otherRepository.GetAll();
            Assert.True(others.Any(o => o.Name == newOther.Name));
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newOther = new Other { Name = "newOther" + DateTime.Now.Ticks, Type = "Something", Custom = true };
            await _otherRepository.AddAsync(newOther);
            var others = await _otherRepository.GetAllAsync();
            Assert.True(others.Any(o => o.Name == newOther.Name));
        }

        [Test]
        public void Add_Hop_Id_Gets_Set()
        {
            var newOther = new Other { Name = "newOther" + DateTime.Now.Ticks, Type = "Something", Custom = true };
            _otherRepository.Add(newOther);
            var other = _otherRepository.GetSingle(newOther.OtherId);
            Assert.NotNull(other);
        }

        [Test]
        public void Update_Get_Updated()
        {
            var other = _otherRepository.GetAll().LastOrDefault();
            other.Name = "Update" + DateTime.Now.Ticks;
            _otherRepository.Update(other);
            var updated = _otherRepository.GetSingle(other.OtherId);
            Assert.AreEqual(other.Name,updated.Name);
        }

        [Test]
        public async Task UpdateAsync_Get_Updated()
        {
            var other = _otherRepository.GetAll().LastOrDefault();
            other.Name = "Update" + DateTime.Now.Ticks;
            await _otherRepository.UpdateAsync(other);
            var updated = await _otherRepository.GetSingleAsync(other.OtherId);
            Assert.AreEqual(other.Name, updated.Name);
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newOther = new Other { Name = "newOther" + DateTime.Now.Ticks, Type = "Something", Custom = true };
            _otherRepository.Add(newOther);
            _otherRepository.Remove(newOther);
            var others = _otherRepository.GetAll();
            Assert.True(others.All(o => o.OtherId != newOther.OtherId));
        }

        [Test]
        public async Task RemoveAsync_Gets_Removed()
        {
            var newOther = new Other { Name = "newOther" + DateTime.Now.Ticks, Type = "Something", Custom = true };
            await _otherRepository.AddAsync(newOther);
            await _otherRepository.RemoveAsync(newOther);
            var others = await _otherRepository.GetAllAsync();
            Assert.True(others.All(o => o.OtherId != newOther.OtherId));
        }
    }
}
