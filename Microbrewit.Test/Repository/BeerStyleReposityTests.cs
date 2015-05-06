using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class BeerStyleReposityTests
    {
        private IBeerStyleRepository _beerStyleRepository;

        [TestFixtureSetUp]
        public void TestFixureSetUp()
        {
            _beerStyleRepository = new BeerStyleDapperRepository();
        }

        [Test]
        public void GetAllBeerStyles_Return_Result()
        {
            var beerStyles = _beerStyleRepository.GetAll();
            Assert.NotNull(beerStyles);
            Assert.True(beerStyles.Any());
        }

        [Test]
        public void GetAll_Return_SuperStyle()
        {
            var beerStyles = _beerStyleRepository.GetAll();
            var beerStyle = beerStyles.FirstOrDefault(b => b.SuperStyleId != null);
            Assert.NotNull(beerStyle.SuperStyle);
        }

        [Test]
        public async Task GetAll_Return_SubStyle()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync();
            Assert.NotNull(beerStyles.FirstOrDefault().SubStyles);
            Assert.True(beerStyles.FirstOrDefault().SubStyles.Any());
        }

        [Test]
        public async Task GetAllAsync_Return_Result()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync();
            Assert.NotNull(beerStyles);
            Assert.True(beerStyles.Any());
        }

        [Test]
        public async Task GetAllAsync_Return_SuperStyle()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync();
            var beerStyle = beerStyles.FirstOrDefault(b => b.SuperStyleId != null);
            Assert.NotNull(beerStyle.SuperStyle);
        }

        [Test]
        public async Task GetAllAsync_Return_SubStyle()
        {
            var beerStyles = await _beerStyleRepository.GetAllAsync();
            Assert.NotNull(beerStyles.FirstOrDefault().SubStyles);
            Assert.True(beerStyles.FirstOrDefault().SubStyles.Any());
        }
      

        [Test]
        public void GetSingle_Returns_Single_BeerStyle()
        {
            var beerStyle = _beerStyleRepository.GetSingle(2);
            Assert.NotNull(beerStyle);
        }


        [Test]
        public void GetSingle_Returns_SuperStyles()
        {
            var beerStyle = _beerStyleRepository.GetSingle(2);
            Assert.NotNull(beerStyle.SuperStyle);
        }

        [Test]
        public void GetSingle_Returns_SubStyles()
        {
            var beerStyle = _beerStyleRepository.GetSingle(1);
            Assert.NotNull(beerStyle.SubStyles);
            Assert.True(beerStyle.SubStyles.Any());
        }

        [Test]
        public async Task GetSingleAsync_Returns_Single_BeerStyle()
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(2);
            Assert.NotNull(beerStyle);
        }


        [Test]
        public async Task GetSingleAsync_Returns_SuperStyles()
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(2);
            Assert.NotNull(beerStyle.SuperStyle);
        }

        [Test]
        public async Task GetSingleAsync_Returns_SubStyles()
        {
            var beerStyle = await _beerStyleRepository.GetSingleAsync(1);
            Assert.NotNull(beerStyle.SubStyles);
            Assert.True(beerStyle.SubStyles.Any());
        }

        [Test]
        public void AddBeerStyle_Gets_Added()
        {
            var newBeerStyle = new BeerStyle
            {
                Name = "Test BeerStyle",
                ABVHigh = 1,
                ABVLow = 5,
                IBUHigh = 1,
                IBULow = 1,
                SRMHigh = 5,
                SRMLow = 3,
                OGLow = 1.001,
                OGHigh = 1.003,
                FGLow = 1.004,
                FGHigh = 1.005,
                Comments = "Notes",
                SuperStyleId = 1
            };
            _beerStyleRepository.Add(newBeerStyle);
            var beerStyle = _beerStyleRepository.GetSingle(newBeerStyle.BeerStyleId);
            Assert.NotNull(beerStyle);
        }

        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newBeerStyle = new BeerStyle
            {
                Name = "Test BeerStyle",
                ABVHigh = 1,
                ABVLow = 5,
                IBUHigh = 1,
                IBULow = 1,
                SRMHigh = 5,
                SRMLow = 3,
                OGLow = 1.001,
                OGHigh = 1.003,
                FGLow = 1.004,
                FGHigh = 1.005,
                Comments = "Notes",
                SuperStyleId = 1
            };
            await _beerStyleRepository.AddAsync(newBeerStyle);
            var beerStyle = await _beerStyleRepository.GetSingleAsync(newBeerStyle.BeerStyleId);
            Assert.NotNull(beerStyle);
        }

        [Test]
        public void Remove_Gets_Deleted()
        {
            var delete = _beerStyleRepository.GetAll().LastOrDefault();
            _beerStyleRepository.Remove(delete);
            var beerStyle = _beerStyleRepository.GetSingle(delete.BeerStyleId);
            Assert.Null(beerStyle);
        }

        [Test]
        public async Task RemoveAsync_Gets_Deleted()
        {
            var delete = _beerStyleRepository.GetAll().LastOrDefault();
            await _beerStyleRepository.RemoveAsync(delete);
            var beerStyle = _beerStyleRepository.GetSingle(delete.BeerStyleId);
            Assert.Null(beerStyle);
        }

        [Test]
        public void Update_Gets_Updated()
        {
            var beerStyle = _beerStyleRepository.GetAll().LastOrDefault();
            beerStyle.Name = "Updated " + DateTime.Now.Ticks;
            _beerStyleRepository.Update(beerStyle);
            
        }

        [Test]
        public async Task UpdateAsync_Gets_Updated()
        {
            var beerStyle = _beerStyleRepository.GetAll().LastOrDefault();
            beerStyle.Name = "Updated " + DateTime.Now.Ticks;
            await _beerStyleRepository.UpdateAsync(beerStyle);

        }

    }
}
