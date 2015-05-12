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
    public class SupplierRepositoryTests
    {
        private ISupplierRepository _supplierRepository;

        [TestFixtureSetUp]
        public void TestFixtureSetUp()
        {
            _supplierRepository = new SupplierDapperRepository();
        }

        [Test]
        public void GetAll_Not_Null_Not_Empty()
        {
            var suppliers = _supplierRepository.GetAll();
            Assert.NotNull(suppliers);
            Assert.True(suppliers.Any());
        }

        [Test]
        public void GetAll_Origin_Included()
        {
            var suppliers = _supplierRepository.GetAll();
            Assert.NotNull(suppliers.FirstOrDefault().Origin);
        }

        [Test]
        public async Task GetAllAsync_Not_Null_Not_Empty()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            Assert.NotNull(suppliers);
            Assert.True(suppliers.Any());
        }

        [Test]
        public async Task GetAllAsync_Origin_Included()
        {
            var suppliers = await _supplierRepository.GetAllAsync();
            Assert.NotNull(suppliers.FirstOrDefault().Origin);
        }

        [Test]
        public void GetSingle_Not_Null_Not_Empty()
        {
            var supplier = _supplierRepository.GetSingle(1);
            Assert.NotNull(supplier);
            Assert.True(supplier.Name.Any());
        }

        [Test]
        public void GetSingle_Origin_Included()
        {
            var supplier = _supplierRepository.GetSingle(1);
            Assert.NotNull(supplier.Origin);
        }

        [Test]
        public async Task GetSingleAsync_Not_Null_Not_Empty()
        {
            var supplier = await _supplierRepository.GetSingleAsync(1);
            Assert.NotNull(supplier);
            Assert.True(supplier.Name.Any());
        }

        [Test]
        public async Task GetSingleAsync_Origin_Included()
        {
            var supplier = await _supplierRepository.GetSingleAsync(1);
            Assert.NotNull(supplier.Origin);
        }

        [Test]
        public void Add_Gets_Added()
        {
            var newSupplier = new Supplier {Name = "newSupplier" + DateTime.Now.Ticks};
            _supplierRepository.Add(newSupplier);
            var suppliers = _supplierRepository.GetAll();
            Assert.True(suppliers.Any(o => o.Name == newSupplier.Name));
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newSupplier = new Supplier { Name = "newSupplier" + DateTime.Now.Ticks};
            await _supplierRepository.AddAsync(newSupplier);
            var suppliers = await _supplierRepository.GetAllAsync();
            Assert.True(suppliers.Any(o => o.Name == newSupplier.Name));
        }

        [Test]
        public void Add_SupplierId_Gets_Set()
        {
            var newSupplier = new Supplier { Name = "newSupplier" + DateTime.Now.Ticks};
            _supplierRepository.Add(newSupplier);
            var supplier = _supplierRepository.GetSingle(newSupplier.SupplierId);
            Assert.NotNull(supplier);
        }

        



        [Test]
        public void Update_Get_Updated()
        {
            var supplier = _supplierRepository.GetAll().LastOrDefault();
            supplier.Name = "Update" + DateTime.Now.Ticks;
            _supplierRepository.Update(supplier);
            var updated = _supplierRepository.GetSingle(supplier.SupplierId);
            Assert.AreEqual(supplier.Name,updated.Name);
        }

        [Test]
        public async Task UpdateAsync_Get_Updated()
        {
            var supplier = _supplierRepository.GetAll().LastOrDefault();
            supplier.Name = "Update" + DateTime.Now.Ticks;
            await _supplierRepository.UpdateAsync(supplier);
            var updated = await _supplierRepository.GetSingleAsync(supplier.SupplierId);
            Assert.AreEqual(supplier.Name, updated.Name);
        }

        [Test]
        public void Remove_Gets_Removed()
        {
            var newSupplier = new Supplier { Name = "newSupplier" + DateTime.Now.Ticks};
            _supplierRepository.Add(newSupplier);
            _supplierRepository.Remove(newSupplier);
            var suppliers = _supplierRepository.GetAll();
            Assert.True(suppliers.All(o => o.SupplierId != newSupplier.SupplierId));
        }

        [Test]
        public async Task RemoveAsync_Gets_Removed()
        {
            var newSupplier = new Supplier { Name = "newSupplier" + DateTime.Now.Ticks};
            await _supplierRepository.AddAsync(newSupplier);
            await _supplierRepository.RemoveAsync(newSupplier);
            var suppliers = await _supplierRepository.GetAllAsync();
            Assert.True(suppliers.All(o => o.SupplierId != newSupplier.SupplierId));
        }
    }
}
