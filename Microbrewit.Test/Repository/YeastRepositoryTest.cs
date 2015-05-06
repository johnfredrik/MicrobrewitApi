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
    public class YeastRepositoryTests
    {
        private IYeastRepository _yeastRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _yeastRepository = new YeastDapperRepository();
        }

        [Test]
        public void GetAll_Not_Null_Not_Empty()
        {
            var yeasts = _yeastRepository.GetAll();
            Assert.NotNull(yeasts);
            Assert.True(yeasts.Any());
        }

        [Test]
        public void GetAll_Supplier_Included()
        {
            var yeasts = _yeastRepository.GetAll();
            Assert.NotNull(yeasts.FirstOrDefault().Supplier);
        }

        [Test]
        public async Task GetAllAsync_Not_Null_Not_Empty()
        {
            var yeasts = await _yeastRepository.GetAllAsync();
            Assert.NotNull(yeasts);
            Assert.True(yeasts.Any());
        }

        [Test]
        public async Task GetAllAsync_Supplier_Included()
        {
            var yeasts = await _yeastRepository.GetAllAsync();
            Assert.NotNull(yeasts.FirstOrDefault().Supplier);
        }

        [Test]
        public void GetSingle_Not_Null_Not_Empty()
        {
            var yeast = _yeastRepository.GetSingle(1);
            Assert.NotNull(yeast);
            Assert.True(yeast.Name.Any());
        }

        [Test]
        public void GetSingle_Supplier_Included()
        {
            var yeast = _yeastRepository.GetSingle(1);
            Assert.NotNull(yeast.Supplier);
        }

        [Test]
        public async Task GetSingleAsync_Not_Null_Not_Empty()
        {
            var yeast = await _yeastRepository.GetSingleAsync(1);
            Assert.NotNull(yeast);
            Assert.True(yeast.Name.Any());
        }

        [Test]
        public async Task GetSingleAsync_Supplier_Included()
        {
            var yeast = await _yeastRepository.GetSingleAsync(1);
            Assert.NotNull(yeast.Supplier);
        }

        [Test]
        public void Add_Gets_Added()
        {
            var newYeast = new Yeast {Name = "newYeast" + DateTime.Now.Ticks,ProductCode = "AAA" ,Type = "Liquid", Custom = true};
            _yeastRepository.Add(newYeast);
            var yeasts = _yeastRepository.GetAll();
            Assert.True(yeasts.Any(o => o.Name == newYeast.Name));
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newYeast = new Yeast { Name = "newYeast" + DateTime.Now.Ticks, ProductCode = "AAA", Type = "Liquid", Custom = true };
            await _yeastRepository.AddAsync(newYeast);
            var yeasts = await _yeastRepository.GetAllAsync();
            Assert.True(yeasts.Any(o => o.Name == newYeast.Name));
        }

        [Test]
        public void Add_YeastId_Gets_Set()
        {
            var newYeast = new Yeast { Name = "newYeast" + DateTime.Now.Ticks, ProductCode = "AAA", Type = "Liquid", Custom = true };
            _yeastRepository.Add(newYeast);
            var yeast = _yeastRepository.GetSingle(newYeast.YeastId);
            Assert.NotNull(yeast);
        }

        



        [Test]
        public void Update_Get_Updated()
        {
            var yeast = _yeastRepository.GetAll().LastOrDefault();
            yeast.Name = "Update" + DateTime.Now.Ticks;
            _yeastRepository.Update(yeast);
            var updated = _yeastRepository.GetSingle(yeast.YeastId);
            Assert.AreEqual(yeast.Name,updated.Name);
        }

        [Test]
        public async Task UpdateAsync_Get_Updated()
        {
            var yeast = _yeastRepository.GetAll().LastOrDefault();
            yeast.Name = "Update" + DateTime.Now.Ticks;
            await _yeastRepository.UpdateAsync(yeast);
            var updated = await _yeastRepository.GetSingleAsync(yeast.YeastId);
            Assert.AreEqual(yeast.Name, updated.Name);
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newYeast = new Yeast { Name = "newYeast" + DateTime.Now.Ticks, ProductCode = "AAA", Type = "Liquid", Custom = true };
            _yeastRepository.Add(newYeast);
            _yeastRepository.Remove(newYeast);
            var yeasts = _yeastRepository.GetAll();
            Assert.True(yeasts.All(o => o.YeastId != newYeast.YeastId));
        }

        [Test]
        public async Task RemoveAsync_Gets_Removed()
        {
            var newYeast = new Yeast { Name = "newYeast" + DateTime.Now.Ticks, ProductCode = "AAA", Type = "Liquid", Custom = true };
            await _yeastRepository.AddAsync(newYeast);
            await _yeastRepository.RemoveAsync(newYeast);
            var yeasts = await _yeastRepository.GetAllAsync();
            Assert.True(yeasts.All(o => o.YeastId != newYeast.YeastId));
        }
    }
}
