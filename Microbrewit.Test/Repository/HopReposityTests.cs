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

namespace Microbrewit.Test
{
    [TestFixture]
    public class HopReposityTests
    {
        private IHopRepository _hopRepository;

        [TestFixtureSetUp]
        public void TestFixureSetUp()
        {
            _hopRepository = new HopDapperRepository();
        }

        [Test]
        public void GetAllHops_Return_Result()
        {
            var hops = _hopRepository.GetAll();
            Assert.NotNull(hops);
            Assert.True(hops.Any());
        }

        [Test]
        public void GetAllHops_Return_Origin_On_Hop()
        {
            var hops = _hopRepository.GetAll();
            var hop = hops.First();
            Assert.NotNull(hop.Origin);
        }

        [Test]
        public void GetAllHops_Return_HopFlavors_On_Hop()
        {
            var hops = _hopRepository.GetAll();
            var hop = hops.First();
            Assert.NotNull(hop.Flavours);
        }

        [Test]
        public void GetAllHops_Return_Substitute_On_Hop()
        {
            var hops = _hopRepository.GetAll();
            var hop = hops.First();
            Assert.NotNull(hop.Substituts);
        }

        [Test]
        public void GetSingle_Returns_Single_Hop()
        {
            var hop = _hopRepository.GetSingle(1);
            Assert.NotNull(hop);
        }

        [Test]
        public void GetSingle_Returns_HopFlavors()
        {
            var hop = _hopRepository.GetSingle(1);
            Assert.NotNull(hop.Flavours);
            Assert.True(hop.Flavours.Any());
        }

        [Test]
        public void GetSingle_Returns_Substitutes()
        {
            var hop = _hopRepository.GetSingle(1);
            Assert.NotNull(hop.Substituts);
            Assert.True(hop.Substituts.Any());
        }

        [Test]
        public void AddHop_Gets_Added()
        {
            var newHop = new Hop
            {
                Name = "Test Hop",
                AALow = 1,
                AAHigh = 5,
                Custom = true,
                BetaLow = 1,
                BetaHigh = 5,
                Notes = "Notes",
                OriginId = 1
            };
            _hopRepository.Add(newHop);
            var hop = _hopRepository.GetSingle(newHop.HopId);
            Assert.NotNull(hop);
        }

        [Test]
        public void AddHop_Hop_Flavour_Gets_Added()
        {
            var newHop = new Hop
            {
                Name = "Test Hop",
                AALow = 1,
                AAHigh = 5,
                Custom = true,
                BetaLow = 1,
                BetaHigh = 5,
                Notes = "Notes",
                OriginId = 1,
                Flavours = new List<HopFlavour> { new HopFlavour { FlavourId = 1} }
            };
            _hopRepository.Add(newHop);
            var hop = _hopRepository.GetSingle(newHop.HopId);
            Assert.True(hop.Flavours.Any());
        }

        [Test]
        public void AddHop_Hop_Substitute_Gets_Added()
        {
            var sub = _hopRepository.GetAll().FirstOrDefault();
            var newHop = new Hop
            {
                Name = "Test Hop",
                AALow = 1,
                AAHigh = 5,
                Custom = true,
                BetaLow = 1,
                BetaHigh = 5,
                Notes = "Notes",
                OriginId = 1,
                Substituts = new List<Hop> {sub}
            };
            _hopRepository.Add(newHop);
            var hop = _hopRepository.GetSingle(newHop.HopId);
            Assert.True(hop.Substituts.Any());
        }

        [Test]
        public void RemoveHop_Gets_Deleted()
        {
            var delete = _hopRepository.GetAll().LastOrDefault();
            _hopRepository.Remove(delete);
            var hop = _hopRepository.GetSingle(delete.HopId);
            Assert.Null(hop);
        }

        [Test]
        public void UpdateHop_Gets_Updated()
        {
            var hop = _hopRepository.GetAll().LastOrDefault();
            hop.Name = "Updated " + DateTime.Now.Ticks;
            _hopRepository.Update(hop);
            
        }

        [Test]
        public void UpdateHop_Falvour_Gets_Added()
        {
            var hop = _hopRepository.GetAll().LastOrDefault();
            hop.Flavours = new List<HopFlavour>();
            _hopRepository.Update(hop);
            hop.Flavours.Add(new HopFlavour {FlavourId = 2, HopId = hop.HopId});
            _hopRepository.Update(hop);
            var updated = _hopRepository.GetSingle(hop.HopId);
            Assert.True(updated.Flavours.Any(f => f.FlavourId == 2));

        }

        [Test]
        public void UpdateHop_Existing_Flavour_Gets_Removed()
        {
            var hop = _hopRepository.GetAll().LastOrDefault();
            hop.Flavours.Add(new HopFlavour { FlavourId = 2, HopId = hop.HopId });
            _hopRepository.Update(hop);
            hop.Flavours = new List<HopFlavour>();
            _hopRepository.Update(hop);
            var updated = _hopRepository.GetSingle(hop.HopId);
            Assert.True(updated.Flavours.All(f => f.FlavourId != 2));

        }

        [Test]
        public void UpdateHop_Substitute_Gets_Added()
        {
            var hops = _hopRepository.GetAll();
            var last = hops.LastOrDefault();
            var first = hops.FirstOrDefault();
            last.Substituts = new List<Hop>();
            _hopRepository.Update(last);
            
            last.Substituts.Add(first);
            _hopRepository.Update(last);
            var updated = _hopRepository.GetSingle(last.HopId);
            Assert.True(updated.Substituts.Any(h => h.HopId == first.HopId));

        }

        [Test]
        public void UpdateHop_Substitute_Gets_Removed()
        {
            var hops = _hopRepository.GetAll();
            var last = hops.LastOrDefault();
            var first = hops.FirstOrDefault();
            last.Substituts.Add(first);
            _hopRepository.Update(last);
            last.Substituts = new List<Hop>();
            _hopRepository.Update(last);
            var updated = _hopRepository.GetSingle(last.HopId);
            Assert.True(updated.Substituts.All(h => h.HopId != first.HopId));

        }

        [Test]
        public void AddFlavour_Gets_Added()
        {
            var flavour = _hopRepository.AddFlavour("Burnt");
            Assert.NotNull(flavour);
            Assert.AreEqual("Burnt",flavour.Name);
        }

        [Test]
        public void GetForms_NotNull_NotEmpty()
        {
            var hopForms = _hopRepository.GetHopForms();
            Assert.NotNull(hopForms);
            Assert.True(hopForms.Any());
        }

        [Test]
        public void GetForm_NotNull_NotEmpty()
        {
            var hopForm = _hopRepository.GetForm(1);
            Assert.NotNull(hopForm);
            Assert.True(hopForm.Name.Any());

        }
    }
}
