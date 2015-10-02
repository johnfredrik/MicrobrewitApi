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
        public async Task AddHop_Gets_Oils_Get_Added()
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
                Purpose = "Test",
                Aliases = "Test;test",
                TotalOilHigh = 1,
                BPineneHigh = 1,
                LinaloolHigh = 1,
                MyrceneHigh = 1,
                CaryophylleneHigh = 1,
                FarneseneHigh = 1,
                HumuleneHigh = 1,
                GeraniolHigh = 1,
                OtherOilHigh = 1,
                TotalOilLow = 1,
                BPineneLow = 1,
                LinaloolLow = 1,
                MyrceneLow = 1,
                CaryophylleneLow = 1,
                FarneseneLow = 1,
                HumuleneLow = 1,
                GeraniolLow = 1,
                OtherOilLow = 1,
                AromaWheel = new List<HopFlavour>
                {
                    new HopFlavour {FlavourId = 1},
                    new HopFlavour {FlavourId = 2}
                },
                Flavours = new List<HopFlavour>
                {
                    new HopFlavour {FlavourId = 1},
                    new HopFlavour {FlavourId = 2}
                }
            };
            await _hopRepository.AddAsync(newHop);
            var hop = _hopRepository.GetSingleAsync(newHop.HopId).Result;
            Assert.AreEqual(newHop.Purpose, hop.Purpose);
            Assert.AreEqual(newHop.Aliases, hop.Aliases);
            Assert.AreEqual(newHop.TotalOilHigh, hop.TotalOilHigh);
            Assert.AreEqual(newHop.BPineneHigh, hop.BPineneHigh);
            Assert.AreEqual(newHop.LinaloolHigh, hop.LinaloolHigh);
            Assert.AreEqual(newHop.MyrceneHigh, hop.MyrceneHigh);
            Assert.AreEqual(newHop.CaryophylleneHigh, hop.CaryophylleneHigh);
            Assert.AreEqual(newHop.FarneseneHigh, hop.FarneseneHigh);
            Assert.AreEqual(newHop.HumuleneHigh, hop.HumuleneHigh);
            Assert.AreEqual(newHop.GeraniolHigh, hop.GeraniolHigh);
            Assert.AreEqual(newHop.OtherOilHigh, hop.OtherOilHigh);
            Assert.AreEqual(newHop.TotalOilLow, hop.TotalOilLow);
            Assert.AreEqual(newHop.BPineneLow, hop.BPineneLow);
            Assert.AreEqual(newHop.LinaloolLow, hop.LinaloolLow);
            Assert.AreEqual(newHop.MyrceneLow, hop.MyrceneLow);
            Assert.AreEqual(newHop.CaryophylleneLow, hop.CaryophylleneLow);
            Assert.AreEqual(newHop.FarneseneLow, hop.FarneseneLow);
            Assert.AreEqual(newHop.HumuleneLow, hop.HumuleneLow);
            Assert.AreEqual(newHop.GeraniolLow, hop.GeraniolLow);
            Assert.AreEqual(newHop.OtherOilLow, hop.OtherOilLow);
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
        public void UpdateHop_Oils_Get_Updated()
        {
            var hop = _hopRepository.GetSingleAsync(172).Result;
            hop.Purpose = "Aroma";
            hop.Aliases = "Test;test";
            hop.TotalOilHigh = 2;
            hop.BPineneHigh = 2;
            hop.LinaloolHigh = 2;
            hop.MyrceneHigh = 2;
            hop.CaryophylleneHigh = 2;
            hop.FarneseneHigh = 2;
            hop.HumuleneHigh = 2;
            hop.GeraniolHigh = 2;
            hop.OtherOilHigh = 2;
            hop.TotalOilLow = 2;
            hop.BPineneLow = 2;
            hop.LinaloolLow = 2;
            hop.MyrceneLow = 2;
            hop.CaryophylleneLow = 2;
            hop.FarneseneLow = 2;
            hop.HumuleneLow = 2;
            hop.GeraniolLow = 2;
            hop.OtherOilLow = 2;
            hop.AromaWheel = new List<HopFlavour>
            {
                new HopFlavour {FlavourId = 1, HopId = hop.HopId},
            };
            hop.Flavours = new List<HopFlavour>
            {
                new HopFlavour {FlavourId = 2, HopId = hop.HopId}
            };
            var result =_hopRepository.UpdateAsync(hop).Result;
            var updatedHop = _hopRepository.GetSingleAsync(172).Result;
            Assert.AreEqual(hop.Purpose, updatedHop.Purpose);
            Assert.AreEqual(hop.Aliases, updatedHop.Aliases);
            Assert.AreEqual(hop.TotalOilHigh, updatedHop.TotalOilHigh);
            Assert.AreEqual(hop.BPineneHigh, updatedHop.BPineneHigh);
            Assert.AreEqual(hop.LinaloolHigh, updatedHop.LinaloolHigh);
            Assert.AreEqual(hop.MyrceneHigh, updatedHop.MyrceneHigh);
            Assert.AreEqual(hop.CaryophylleneHigh, updatedHop.CaryophylleneHigh);
            Assert.AreEqual(hop.FarneseneHigh, updatedHop.FarneseneHigh);
            Assert.AreEqual(hop.HumuleneHigh, updatedHop.HumuleneHigh);
            Assert.AreEqual(hop.GeraniolHigh, updatedHop.GeraniolHigh);
            Assert.AreEqual(hop.OtherOilHigh, updatedHop.OtherOilHigh);
            Assert.AreEqual(hop.TotalOilLow, updatedHop.TotalOilLow);
            Assert.AreEqual(hop.BPineneLow, updatedHop.BPineneLow);
            Assert.AreEqual(hop.LinaloolLow, updatedHop.LinaloolLow);
            Assert.AreEqual(hop.MyrceneLow, updatedHop.MyrceneLow);
            Assert.AreEqual(hop.CaryophylleneLow, updatedHop.CaryophylleneLow);
            Assert.AreEqual(hop.FarneseneLow, updatedHop.FarneseneLow);
            Assert.AreEqual(hop.HumuleneLow, updatedHop.HumuleneLow);
            Assert.AreEqual(hop.GeraniolLow, updatedHop.GeraniolLow);
            Assert.AreEqual(hop.OtherOilLow, updatedHop.OtherOilLow);
            Assert.NotNull(updatedHop);
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
