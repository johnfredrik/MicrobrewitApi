using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Policy;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository;
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Automapper.CustomResolvers;
using Microbrewit.Service.Interface;
using NUnit.Framework;

namespace Microbrewit.Test.Repository
{
    [TestFixture]
    public class UserRepositoryTests
    {
        private IUserRepository _userRepository;

        [TestFixtureSetUp]
        public void TestFixureSetup()
        {
            _userRepository = new UserDapperRepository();
        }

        [Test]
        public void GetAll_NotNull_NotEmpty()
        {
            var users = _userRepository.GetAll();
            Assert.NotNull(users);
            Assert.True(users.Any());
        }

        [Test]
        public void GetAll_UserSocials_Inclued()
        {
            var users = _userRepository.GetAll();
            Assert.True(users.Any(u => u.Socials.Any()));
        }

        [Test]
        public void GetAll_BreweryMember_Inclued()
        {
            var users = _userRepository.GetAll();
            Assert.True(users.Any(u => u.Breweries.Any()));
        }

        [Test]
        public void GetAll_UserBeers_Inclued()
        {
            var users = _userRepository.GetAll();
            Assert.True(users.Any(u => u.Beers.Any()));
        }

        //GetAllAsync
        [Test]
        public async Task GetAllAsync_NotNull_NotEmpty()
        {
            var users = await _userRepository.GetAllAsync();
            Assert.NotNull(users);
            Assert.True(users.Any());
        }

        [Test]
        public async Task GetAllAsync_UserSocials_Inclued()
        {
            var users = await _userRepository.GetAllAsync();
            Assert.True(users.Any(u => u.Socials.Any()));
        }

        [Test]
        public async Task GetAllAsync_BreweryMember_Inclued()
        {
            var users = await _userRepository.GetAllAsync();
            Assert.True(users.Any(u => u.Breweries.Any()));
        }

        [Test]
        public async Task GetAllAsync_UserBeers_Inclued()
        {
            var users = await _userRepository.GetAllAsync();
            Assert.True(users.Any(u => u.Beers.Any()));
        }

        //GetSingle
        [Test]
        public void GetSingle_NotNull_NotEmpty()
        {
            var user = _userRepository.GetSingle("johnfredrik");
            Assert.NotNull(user);
            Assert.True(user.Username.Any());
        }

        [Test]
        public void GetSingle_UserSocials_Inclued()
        {
            var user = _userRepository.GetSingle("johnfredrik");
            Assert.True(user.Socials.Any());
        }

        [Test]
        public void GetSingle_BreweryMember_Inclued()
        {
            var user = _userRepository.GetSingle("johnfredrik");
            Assert.True(user.Breweries.Any());
        }

        [Test]
        public void GetSingle_UserBeers_Inclued()
        {
            var user = _userRepository.GetSingle("johnfredrik");
            Assert.True(user.Beers.Any());
        }

        //GetSingleAsync
        [Test]
        public async Task GetSingleAsync_NotNull_NotEmpty()
        {
            var user = await _userRepository.GetSingleAsync("johnfredrik");
            Assert.NotNull(user);
            Assert.True(user.Username.Any());
        }

        [Test]
        public async Task GetSingleAsync_UserSocials_Inclued()
        {
            var user = await _userRepository.GetSingleAsync("johnfredrik");
            Assert.True(user.Socials.Any());
        }

        [Test]
        public async Task GetSingleAsync_BreweryMember_Inclued()
        {
            var user = await _userRepository.GetSingleAsync("johnfredrik");
            Assert.True(user.Breweries.Any());
        }

        [Test]
        public async Task GetSingleAsync_UserBeers_Inclued()
        {
            var user = await _userRepository.GetSingleAsync("johnfredrik");
            Assert.True(user.Beers.Any());
        }

        //Add
        [Test]
        public void Add_Gets_Added()
        {
            var newUser = new User {Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io"};
            _userRepository.Add(newUser);
            var users = _userRepository.GetAll();
            Assert.True(users.Any(u => u.Username == newUser.Username));
        }

        [Test]
        public void Add_Socials_Gets_Added()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io"};
            newUser.Socials =  new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            _userRepository.Add(newUser);
            var user = _userRepository.GetSingle(newUser.Username);
            Assert.True(user.Socials.Any());
        }

        //AddAsync
        [Test]
        public async Task AddAsync_Gets_Added()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            await _userRepository.AddAsync(newUser);
            var users = await _userRepository.GetAllAsync();
            Assert.True(users.Any(u => u.Username == newUser.Username));
        }

        [Test]
        public async Task AddAsync_Socials_Gets_Added()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            await _userRepository.AddAsync(newUser);
            var user = await _userRepository.GetSingleAsync(newUser.Username);
            Assert.True(user.Socials.Any());
        }

        //Update
        [Test]
        public void Update_Gets_Updated()
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            user.HeaderImage = "http://image.jpg";
            _userRepository.Update(user);
            var updated = _userRepository.GetSingle(user.Username);
            Assert.AreEqual(updated.HeaderImage,user.HeaderImage);
        }

        [Test]
        public void Update_Social_Gets_Added()
        {
            var user = _userRepository.GetAll().FirstOrDefault();
            var social = new UserSocial {Site = "Twitter", Url = "@" + user.Username};
            user.Socials.Add(social);
            _userRepository.Update(user);
            var updated = _userRepository.GetSingle(user.Username);
            Assert.True(updated.Socials.Any(s => s.Site == social.Site && s.Url == social.Url));
        }

        [Test]
        public void Update_Social_Gets_Updated()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
             _userRepository.Add(newUser);
            var user =  _userRepository.GetSingle(newUser.Username);
            var social = user.Socials.FirstOrDefault();
            social.Url = "@" + DateTime.Now.Ticks;
            _userRepository.Update(user);
            var updated = _userRepository.GetSingle(user.Username);
            Assert.AreEqual(updated.Socials.FirstOrDefault().Url,social.Url);
        }

        [Test]
        public void Update_Social_All_Gets_Removed()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            _userRepository.Add(newUser);
            var user = _userRepository.GetSingle(newUser.Username);
            user.Socials = new List<UserSocial>();
            _userRepository.Update(user);
            var updated = _userRepository.GetSingle(user.Username);
            Assert.AreEqual(0, updated.Socials.Count);
        }

        [Test]
        public void Update_Social_One_Gets_Removed()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            _userRepository.Add(newUser);
            var user = _userRepository.GetSingle(newUser.Username);
            user.Socials.Remove(user.Socials.FirstOrDefault());
            _userRepository.Update(user);
            var updated = _userRepository.GetSingle(user.Username);
            Assert.AreEqual(1, updated.Socials.Count);
        }

        //UpdateAsync
        [Test]
        public async Task UpdateAsync_Gets_Updated()
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault();
            user.HeaderImage = "http://image.jpg";
            await _userRepository.UpdateAsync(user);
            var updated = await _userRepository.GetSingleAsync(user.Username);
            Assert.AreEqual(updated.HeaderImage, user.HeaderImage);
        }

        [Test]
        public async Task UpdateAsync_Social_Gets_Added()
        {
            var users = await _userRepository.GetAllAsync();
            var user = users.FirstOrDefault();
            var social = new UserSocial { Site = "Twitter", Url = "@" + user.Username };
            user.Socials.Add(social);
            await _userRepository.UpdateAsync(user);
            var updated = await _userRepository.GetSingleAsync(user.Username);
            Assert.True(updated.Socials.Any(s => s.Site == social.Site && s.Url == social.Url));
        }

        [Test]
        public async Task UpdateAsync_Social_Gets_Updated()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            await _userRepository.AddAsync(newUser);
            var user = await _userRepository.GetSingleAsync(newUser.Username);
            var social = user.Socials.FirstOrDefault();
            social.Url = "@" + DateTime.Now.Ticks;
            await _userRepository.UpdateAsync(user);
            var updated = await _userRepository.GetSingleAsync(user.Username);
            Assert.AreEqual(updated.Socials.FirstOrDefault().Url, social.Url);
        }

        [Test]
        public async Task UpdateAsync_Social_All_Gets_Removed()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            await _userRepository.AddAsync(newUser);
            var user = await _userRepository.GetSingleAsync(newUser.Username);
            user.Socials = new List<UserSocial>();
            await _userRepository.UpdateAsync(user);
            var updated = await _userRepository.GetSingleAsync(user.Username);
            Assert.AreEqual(0, updated.Socials.Count);
        }

        [Test]
        public async Task UpdateAsync_Social_One_Gets_Removed()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            newUser.Socials = new List<UserSocial>
            {
                new UserSocial {Site="Homepage",Url = "http://test.com/" + newUser.Username},
                new UserSocial {Site="Twitter",Url = "@" + newUser.Username}
            };
            await _userRepository.AddAsync(newUser);
            var user = await _userRepository.GetSingleAsync(newUser.Username);
            user.Socials.Remove(user.Socials.FirstOrDefault());
            await _userRepository.UpdateAsync(user);
            var updated = await _userRepository.GetSingleAsync(user.Username);
            Assert.AreEqual(1, updated.Socials.Count);
        }

        //Remove
        [Test]
        public void Remove_Gets_Removed()
        {
            var newUser = new User { Username = "NewUser" + DateTime.Now.Ticks, Email = "knall@asphaug.io" };
            _userRepository.Add(newUser);
            var user = _userRepository.GetSingle(newUser.Username);
            _userRepository.Remove(user);
            var removed = _userRepository.GetSingle(newUser.Username);
            Assert.Null(removed);
        }

        //GetUserSocials
        [Test]
        public void GetUserSocials_Return()
        {
            var userSocials = _userRepository.GetUserSocials("johnfredrik");
            Assert.NotNull(userSocials);
            Assert.True(userSocials.Any());
        }

        //GetUserBeersAsync
        [Test]
        public async Task GetUserBeersAsync_Return()
        {
            var userBeers = await _userRepository.GetAllUserBeersAsync("johnfredrik");
            Assert.NotNull(userBeers);
            Assert.True(userBeers.Any());
        }

        [Test]
        public async Task ConfirmUserBeerAsync_Return_True()
        {
            var notification = new NotificationDto {Id = 26, Value = true};
            var confirmed = await _userRepository.ConfirmUserBeerAsync("johnfredrik", notification);
            Assert.True(confirmed);
        }

        [Test]
        public async Task ConfirmUserBeerAsync_Return_False()
        {
            var notification = new NotificationDto { Id = 27, Value = false };
            var confirmed = await _userRepository.ConfirmUserBeerAsync("johnfredrik", notification);
            Assert.True(confirmed);
        }


        [Test]
        public async Task ConfirmUserBeerAsync_WrongId_Return_False()
        {
            var notification = new NotificationDto { Id = int.MaxValue, Value = true };
            var confirmed = await _userRepository.ConfirmUserBeerAsync("johnfredrik", notification);
            Assert.False(confirmed);
        }

        //ConfirmBreweryMemberAsync
        [Test]
        public async Task ConfirmBreweryMemberAsync_Return_True()
        {
            var notification = new NotificationDto { Id = 13, Value = true };
            var confirmed = await _userRepository.ConfirmBreweryMemberAsync("johnfredrik", notification);
            Assert.True(confirmed);
        }

        [Test]
        public async Task ConfirmBreweryMemberAsync_Return_False()
        {
            var notification = new NotificationDto { Id = 13, Value = false };
            var confirmed = await _userRepository.ConfirmBreweryMemberAsync("johnfredrik", notification);
            Assert.True(confirmed);
        }


        [Test]
        public async Task ConfirmBreweryMemberAsync_WrongId_Return_False()
        {
            var notification = new NotificationDto { Id = int.MaxValue, Value = true };
            var confirmed = await _userRepository.ConfirmBreweryMemberAsync("johnfredrik", notification);
            Assert.False(confirmed);
        }


    }
}
