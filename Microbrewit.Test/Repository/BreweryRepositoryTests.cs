using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.CompilerServices;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class BreweryRepositoryTests
    {
        private IBreweryRepository _breweryRepository;

        [TestFixtureSetUp]
        public void TestFixureSetup()
        {
            _breweryRepository = new BreweryDapperRepository();
        }

        [Test]
        public void GetAll_Returns()
        {
            var breweries = _breweryRepository.GetAll();
            Assert.NotNull(breweries);
            Assert.True(breweries.Any());
        }

        [Test]
        public async Task GetAllAsync_Returns()
        {
            var breweries = await _breweryRepository.GetAllAsync();
            Assert.NotNull(breweries);
            Assert.True(breweries.Any());
        }

        [Test]
        public async Task GetSingleAsync_Returns()
        {
            var brewery = await _breweryRepository.GetSingleAsync(13);
            Assert.NotNull(brewery);
            Assert.True(brewery.Name.Any());
        }

        [Test]
        public void Add_Get_Added()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            _breweryRepository.Add(newBrewery);
            var breweries = _breweryRepository.GetAll();
            Assert.True(breweries.Any(b => b.Name == newBrewery.Name));
        }

        [Test]
        public async Task AddAsync_Get_Added()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            await _breweryRepository.AddAsync(newBrewery);
            var breweries = await _breweryRepository.GetAllAsync();
            Assert.True(breweries.Any(b => b.Name == newBrewery.Name));
        }


        [Test]
        public void Add_Brewery_Socials()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            var brewerySocials = new List<BrewerySocial>
            {
                new BrewerySocial{Site = "Twitter", Url = "@" + newBrewery.Name},
                new BrewerySocial{Site = "Fanpage", Url = "http://fanpage.com/" + newBrewery.Name},
            };
            newBrewery.Socials = brewerySocials;

            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            Assert.True(brewery.Socials.Any());
        }

        [Test]
        public async Task AddAsync_Brewery_Socials()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            var brewerySocials = new List<BrewerySocial>
            {
                new BrewerySocial{Site = "Twitter", Url = "@" + newBrewery.Name},
                new BrewerySocial{Site = "Fanpage", Url = "http://fanpage.com/" + newBrewery.Name},
            };
            newBrewery.Socials = brewerySocials;

            await _breweryRepository.AddAsync(newBrewery);
            var brewery = await _breweryRepository.GetSingleAsync(newBrewery.BreweryId);
            Assert.True(brewery.Socials.Any());
        }

        [Test]
        public void Add_BreweryMembers()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            var breweryMembers = new List<BreweryMember>
            {
                new BreweryMember{MemberUsername = "johnfredrik",Role = "Member"},
                new BreweryMember{MemberUsername = "torstein",Role = "Member"},
            };
            newBrewery.Members = breweryMembers ;

            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            Assert.True(brewery.Members.Any());
        }

        [Test]
        public async Task AddAsync_BreweryMembers()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            var breweryMembers = new List<BreweryMember>
            {
                new BreweryMember{MemberUsername = "johnfredrik",Role = "Member"},
                new BreweryMember{MemberUsername = "torstein",Role = "Member"},
            };
            newBrewery.Members = breweryMembers;

            await _breweryRepository.AddAsync(newBrewery);
            var brewery = await _breweryRepository.GetSingleAsync(newBrewery.BreweryId);
            Assert.True(brewery.Members.Any());
        }

        [Test]
        public void Update_Gets_Updated()
        {
            var brewery = _breweryRepository.GetAll().LastOrDefault();
            brewery.Name = "Updated" + DateTime.Now;
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(brewery.Name,updated.Name);
        }

        [Test]
        public void Update_Social_Gets_Added()
        {
            var brewery = _breweryRepository.GetAll().FirstOrDefault();
            var social = new BrewerySocial { Site = "Twitter", Url = "@" + brewery.Name };
            brewery.Socials.Add(social);
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.True(updated.Socials.Any(s => s.Site == social.Site && s.Url == social.Url));
        }

        [Test]
        public void Update_Social_Gets_Updated()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Socials = new List<BrewerySocial>
            {
                new BrewerySocial {Site="Homepage",Url = "http://test.com/" + newBrewery.Name},
                new BrewerySocial {Site="Twitter",Url = "@" + newBrewery.Name}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            var social = brewery.Socials.FirstOrDefault();
            social.Url = "@" + DateTime.Now.Ticks;
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(updated.Socials.FirstOrDefault().Url, social.Url);
        }

        [Test]
        public void Update_Social_All_Gets_Removed()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Socials = new List<BrewerySocial>
            {
                new BrewerySocial {Site="Homepage",Url = "http://test.com/" + newBrewery.BreweryId},
                new BrewerySocial {Site="Twitter",Url = "@" + newBrewery.BreweryId}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            brewery.Socials = new List<BrewerySocial>();
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(0, updated.Socials.Count);
        }

        [Test]
        public void Update_Social_One_Gets_Removed()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Socials = new List<BrewerySocial>
            {
                new BrewerySocial {Site="Homepage",Url = "http://test.com/" + newBrewery.Name},
                new BrewerySocial {Site="Twitter",Url = "@" + newBrewery.Name}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            brewery.Socials.Remove(brewery.Socials.FirstOrDefault());
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(1, updated.Socials.Count);
        }

        [Test]
        public void Update_BreweryMember_Gets_Added()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            var breweryMember = new BreweryMember { MemberUsername = "johnfredrik"};
            brewery.Members.Add(breweryMember);
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.True(updated.Members.Any(s => s.MemberUsername == brewery.Members.FirstOrDefault().MemberUsername && s.BreweryId == brewery.Members.FirstOrDefault().BreweryId));
        }

        [Test]
        public void Update_Members_Gets_Updated()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Members = new List<BreweryMember>
            {
                new BreweryMember {MemberUsername = "johnfredrik"},
                new BreweryMember {MemberUsername = "torstein"}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            var member = brewery.Members.FirstOrDefault();
            member.Role = DateTime.Now.Ticks.ToString();
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(updated.Members.FirstOrDefault().Role, member.Role);
        }

        [Test]
        public void Update_Members_All_Gets_Removed()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Members = new List<BreweryMember>
            {
                new BreweryMember {MemberUsername = "johnfredrik"},
                new BreweryMember {MemberUsername = "torstein"}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            brewery.Members = new List<BreweryMember>();
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(0, updated.Members.Count);
        }

        [Test]
        public void Update_Members_One_Gets_Removed()
        {
            var newBrewery = new Brewery
            {
                Name = "NewBrewery" + DateTime.Now.Ticks,
                Description = "Somehting",
                Type = "HomeBrewery",
                OriginId = 5,
                Address = "Something",
                Latitude = 60.3894,
                Longitude = 5.33,
                Website = "Something",
                HeaderImage = "Something",
                Avatar = "Something",
            };
            newBrewery.Members = new List<BreweryMember>
            {
                new BreweryMember {MemberUsername = "johnfredrik"},
                new BreweryMember {MemberUsername = "torstein"}
            };
            _breweryRepository.Add(newBrewery);
            var brewery = _breweryRepository.GetSingle(newBrewery.BreweryId);
            brewery.Members.Remove(brewery.Members.FirstOrDefault());
            _breweryRepository.Update(brewery);
            var updated = _breweryRepository.GetSingle(brewery.BreweryId);
            Assert.AreEqual(1, updated.Members.Count);
        }

        [Test]
        public async Task GetSingleMemberAsync_Gets_Results()
        {
            var member = await _breweryRepository.GetSingleMemberAsync(29, "johnfredrik");
            Assert.NotNull(member);
        }

        [Test]
        public async Task GetAllMemberAsync_Gets_Results()
        {
            var members = await _breweryRepository.GetAllMembersAsync(29);
            Assert.NotNull(members);
            Assert.AreEqual(2,members.Count);
        }

    }
}
