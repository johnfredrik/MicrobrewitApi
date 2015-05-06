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
    public class FermentableRepositoryTests
    {
        private IFermentableRepository _fermentableRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _fermentableRepository = new FermentableDapperRepository();
        }

        [Test]
        public void GetAll_Not_Null_Not_Empty()
        {
            var fermentables = _fermentableRepository.GetAll();
            Assert.NotNull(fermentables);
            Assert.True(fermentables.Any());
        }

        [Test]
        public void GetAll_Supplier_Included()
        {
            var fermentables = _fermentableRepository.GetAll();
            Assert.True(fermentables.Any(f => f.SupplierId != null));
            Assert.True(fermentables.Any(f => f.Supplier != null));
        }

        [Test]
        public void GetAll_SuperFermentable_Included()
        {
            var fermentables = _fermentableRepository.GetAll();
            Assert.True(fermentables.Any(f => f.SuperFermentableId != null));
            Assert.True(fermentables.Any(f => f.SuperFermentable != null));
        }

        [Test]
        public void GetAll_SubFermentable_Included()
        {
            var fermentables = _fermentableRepository.GetAll();
            Assert.True(fermentables.Any(f => f.SubFermentables.Any()));
        }

        //GetAllAsync
        [Test]
        public async Task GetAllAsync_Not_Null_Not_Empty()
        {
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.NotNull(fermentables);
            Assert.True(fermentables.Any());
        }

        [Test]
        public async Task GetAllAsync_Supplier_Included()
        {
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.True(fermentables.Any(f => f.SupplierId != null));
            Assert.True(fermentables.Any(f => f.Supplier != null));
        }

        [Test]
        public async Task GetAllAsync_SuperFermentable_Included()
        {
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.True(fermentables.Any(f => f.SuperFermentableId != null));
            Assert.True(fermentables.Any(f => f.SuperFermentable != null));
        }

        [Test]
        public async Task GetAllAsync_SubFermentable_Included()
        {
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.True(fermentables.Any(f => f.SubFermentables.Any()));
        }


        // GetSingle
        [Test]
        public void GetSingle_Not_Null_Not_Empty()
        {
            var fermentable = _fermentableRepository.GetSingle(1);
            Assert.NotNull(fermentable);
            Assert.True(fermentable.Name.Any());
        }

        [Test]
        public void GetSingle_Supplier_Included()
        {
            var fermentable = _fermentableRepository.GetSingle(1);
            Assert.NotNull(fermentable.SupplierId);
            Assert.NotNull(fermentable.Supplier);
        }

        [Test]
        public void GetSingle_SuperFermentable_Included()
        {
            var fermentable = _fermentableRepository.GetSingle(2);
            Assert.True(fermentable.SuperFermentableId != null);
            Assert.True(fermentable.SuperFermentable != null);
        }

        [Test]
        public void GetSingle_SubFermentable_Included()
        {
            var fermentable = _fermentableRepository.GetSingle(1);
            Assert.True(fermentable.SubFermentables.Any());
        }

        //GetSingleAsync
        [Test]
        public async Task GetSingleAsync_Not_Null_Not_Empty()
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(1);
            Assert.NotNull(fermentable);
            Assert.True(fermentable.Name.Any());
        }

        [Test]
        public async Task GetSingleAsync_Supplier_Included()
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(1);
            Assert.NotNull(fermentable.SupplierId);
            Assert.NotNull(fermentable.Supplier);
        }

        [Test]
        public async Task GetSingleAsync_SuperFermentable_Included()
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(2);
            Assert.True(fermentable.SuperFermentableId != null);
            Assert.True(fermentable.SuperFermentable != null);
        }

        [Test]
        public async Task GetSingleAsync_SubFermentable_Included()
        {
            var fermentable = await _fermentableRepository.GetSingleAsync(1);
            Assert.True(fermentable.SubFermentables.Any());
        }

        //Add
        [Test]
        public void Add_Gets_Added()
        {
            var newFermentable = new Fermentable {Name = "newFermentable" + DateTime.Now.Ticks, Type = "Grain", Custom = true};
            _fermentableRepository.Add(newFermentable);
            var fermentables = _fermentableRepository.GetAll();
            Assert.True(fermentables.Any(o => o.Name == newFermentable.Name));
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newFermentable = new Fermentable { Name = "newFermentable" + DateTime.Now.Ticks, Type = "Grain", Custom = true };
            await _fermentableRepository.AddAsync(newFermentable);
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.True(fermentables.Any(o => o.Name == newFermentable.Name));
        }

        [Test]
        public void Add_FermentableId_Gets_Set()
        {
            var newFermentable = new Fermentable { Name = "newFermentable" + DateTime.Now.Ticks, Type = "Grain", Custom = true };
            _fermentableRepository.Add(newFermentable);
            var fermentable = _fermentableRepository.GetSingle(newFermentable.FermentableId);
            Assert.NotNull(fermentable);
        }

        



        [Test]
        public void Update_Get_Updated()
        {
            var fermentable = _fermentableRepository.GetAll().LastOrDefault();
            fermentable.Name = "Update" + DateTime.Now.Ticks;
            _fermentableRepository.Update(fermentable);
            var updated = _fermentableRepository.GetSingle(fermentable.FermentableId);
            Assert.AreEqual(fermentable.Name,updated.Name);
        }

        [Test]
        public async Task UpdateAsync_Get_Updated()
        {
            var fermentable = _fermentableRepository.GetAll().LastOrDefault();
            fermentable.Name = "Update" + DateTime.Now.Ticks;
            await _fermentableRepository.UpdateAsync(fermentable);
            var updated = await _fermentableRepository.GetSingleAsync(fermentable.FermentableId);
            Assert.AreEqual(fermentable.Name, updated.Name);
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newFermentable = new Fermentable { Name = "newFermentable" + DateTime.Now.Ticks, Type = "Grain", Custom = true };
            _fermentableRepository.Add(newFermentable);
            _fermentableRepository.Remove(newFermentable);
            var fermentables = _fermentableRepository.GetAll();
            Assert.True(fermentables.All(o => o.FermentableId != newFermentable.FermentableId));
        }

        [Test]
        public async Task RemoveAsync_Gets_Removed()
        {
            var newFermentable = new Fermentable { Name = "newFermentable" + DateTime.Now.Ticks, Type = "Grain", Custom = true };
            await _fermentableRepository.AddAsync(newFermentable);
            await _fermentableRepository.RemoveAsync(newFermentable);
            var fermentables = await _fermentableRepository.GetAllAsync();
            Assert.True(fermentables.All(o => o.FermentableId != newFermentable.FermentableId));
        }
    }
}
