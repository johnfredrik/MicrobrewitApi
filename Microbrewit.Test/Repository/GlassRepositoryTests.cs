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
    public class GlassRepositoryTests
    {
        private IGlassRepository _glassRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _glassRepository = new GlassDapperRepository();
        }

        [Test]
        public void GetAll_Not_Null_Not_Empty()
        {
            var glasses = _glassRepository.GetAll();
            Assert.NotNull(glasses);
            Assert.True(glasses.Any());
        }

        [Test]
        public async Task GetAllAsync_Not_Null_Not_Empty()
        {
            var glasses = await _glassRepository.GetAllAsync();
            Assert.NotNull(glasses);
            Assert.True(glasses.Any());
        }

        [Test]
        public void GetSingle_Not_Null_Not_Empty()
        {
            var glass = _glassRepository.GetSingle(1);
            Assert.NotNull(glass);
            Assert.True(glass.Name.Any());
        }

        [Test]
        public async Task GetSingleAsync_Not_Null_Not_Empty()
        {
            var glass = await _glassRepository.GetSingleAsync(1);
            Assert.NotNull(glass);
            Assert.True(glass.Name.Any());
        }

        [Test]
        public void Add_Gets_Added()
        {
            var newGlass = new Glass {Name = "newGlass" + DateTime.Now.Ticks};
            _glassRepository.Add(newGlass);
            var glasses = _glassRepository.GetAll();
            Assert.True(glasses.Any(o => o.Name == newGlass.Name));
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newGlass = new Glass { Name = "newGlass" + DateTime.Now.Ticks};
            await _glassRepository.AddAsync(newGlass);
            var glasses = await _glassRepository.GetAllAsync();
            Assert.True(glasses.Any(o => o.Name == newGlass.Name));
        }

        [Test]
        public void Add_Hop_Id_Gets_Set()
        {
            var newGlass = new Glass { Name = "newGlass" + DateTime.Now.Ticks};
            _glassRepository.Add(newGlass);
            var other = _glassRepository.GetSingle(newGlass.GlassId);
            Assert.NotNull(other);
        }

        [Test]
        public void Update_Get_Updated()
        {
            var glass = _glassRepository.GetAll().LastOrDefault();
            glass.Name = "Update" + DateTime.Now.Ticks;
            _glassRepository.Update(glass);
            var updated = _glassRepository.GetSingle(glass.GlassId);
            Assert.AreEqual(glass.Name,updated.Name);
        }

        [Test]
        public async Task UpdateAsync_Get_Updated()
        {
            var glass = _glassRepository.GetAll().LastOrDefault();
            glass.Name = "Update" + DateTime.Now.Ticks;
            await _glassRepository.UpdateAsync(glass);
            var updated = await _glassRepository.GetSingleAsync(glass.GlassId);
            Assert.AreEqual(glass.Name, updated.Name);
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newGlass = new Glass { Name = "newGlass" + DateTime.Now.Ticks};
            _glassRepository.Add(newGlass);
            _glassRepository.Remove(newGlass);
            var glasses = _glassRepository.GetAll();
            Assert.True(glasses.All(o => o.GlassId != newGlass.GlassId));
        }

        [Test]
        public async Task RemoveAsync_Gets_Removed()
        {
            var newGlass = new Glass { Name = "newGlass" + DateTime.Now.Ticks};
            await _glassRepository.AddAsync(newGlass);
            await _glassRepository.RemoveAsync(newGlass);
            var glasses = await _glassRepository.GetAllAsync();
            Assert.True(glasses.All(o => o.GlassId != newGlass.GlassId));
        }
    }
}
