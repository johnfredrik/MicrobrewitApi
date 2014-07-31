using System;
using System.Collections.Generic;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Api.Util;
using Microbrewit.Repository;
using log4net;
using System.Web;
using System.Security.Principal;
using AutoMapper;
using StackExchange.Redis;
using System.Text;
using Microbrewit.Api.ValidationAttribute;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IUserRepository _userRepository;
        private Elasticsearch.ElasticSearch _elasticsearch;
        private readonly IUserCredentialRepository _userCredentialsRepsoitory;
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserController(IUserRepository userRepository, IUserCredentialRepository userCredentialsRepsoitory)
        {
            this._userRepository = userRepository;
            this._elasticsearch = new Elasticsearch.ElasticSearch();
            this._userCredentialsRepsoitory = userCredentialsRepsoitory;
        }

        // GET api/User
        [TokenValidation]
        [Route("")]
        public UserCompleteDto GetUsers()
        {
            var temp = _userRepository.GetAll("Breweries.Brewery","Beers.Beer");
            var users = Mapper.Map<IList<User>, IList<UserDto>>(temp);
            var result = new UserCompleteDto();
            result.Users = users;
            return result;
        }

        // GET api/User/5
        [Route("{username}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string username)
        {
            var user = Mapper.Map<User, UserDto>(_userRepository.GetSingle(u => u.Username.Equals(username), "Breweries.Brewery", "Beers.Beer"));
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserCompleteDto() { Users = new List<UserDto>() };
            result.Users.Add(user);
            return Ok(result);
        }

        // PUT api/User/5
        [Route("{username}")]
        public async Task<IHttpActionResult> PutUser(string username, UserPostDto userPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!username.Equals(userPostDto.Username))
            {
                return BadRequest();
            }
            if (userPostDto.Password != null)
            {

                var userCredentials = await _userCredentialsRepsoitory.GetSingleAsync(u => u.Username.Equals(userPostDto.Username));
                var salt = Encrypting.GenerateSalt();
                var hashedPassword = Encrypting.Hash(userPostDto.Password, salt);
                userCredentials.Password = hashedPassword;
                userCredentials.Salt = salt;
                await _userCredentialsRepsoitory.UpdateAsync(userCredentials);

            }

            var user = Mapper.Map<UserPostDto,User>(userPostDto);
            await _userRepository.UpdateAsync(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Takes username and password in base 
        /// </summary>
        /// <returns></returns>
        [LoginValidation]
        [Route("login")]
        public async Task<IHttpActionResult> PostLogin(HttpRequestMessage response)
        {                 
            var authorization = Encoding.UTF8.GetString(Convert.FromBase64String(response.Headers.Authorization.Parameter));
            var username = authorization.Split(':').First();
            var user = await _userRepository.GetSingleAsync(u => u.Username.Equals(username), "Breweries.Brewery");          
            var userDto =  Mapper.Map<User,UserDto>(user);
            return Ok(userDto);
        }

        [TokenInvalidation]
        [Route("logout")]
        [HttpPost]
        public IHttpActionResult PostLogout()
        {
            return Ok();
        }

        // POST api/User
        [Route("")]
        [ResponseType(typeof(UserPostDto))]
        public IHttpActionResult PostUser(UserPostDto userPostDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var password = userPostDto.Password;
            var salt = Encrypting.GenerateSalt();
            var hashedPassword = Encrypting.Hash(password,salt);
                       
            var user = Mapper.Map<UserPostDto,User>(userPostDto);
            var userCredential = new UserCredentials() { Password = hashedPassword, Salt = salt, Username = user.Username, User = user };
            _userCredentialsRepsoitory.Add(userCredential);

            var result = new UserCompleteDto() { Users = new List<UserDto>()};
         
            result.Users.Add(Mapper.Map<User,UserDto>(user));

            return CreatedAtRoute("DefaultApi", new { controller = "users", id = user.Username }, result);
        }

        // DELETE api/User/5
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> DeleteUser(int id)
        {
            User user = await db.Users.FindAsync(id);
            if (user == null)
            {
                return NotFound();
            }

            db.Users.Remove(user);
            await db.SaveChangesAsync();

            return Ok(user);
        }

        /// <summary>
        /// Updates users to elasticsearch.
        /// </summary>
        /// <returns></returns>
        [HttpGet]
        [Route("update")]
        public async Task<IHttpActionResult> UpdateBrewery()
        {
            var users = await _userRepository.GetAllAsync("Breweries.Brewery", "Beers.Beer");
            var usersDto = Mapper.Map<IList<User>, IList<UserDto>>(users);
            await _elasticsearch.UpdateUsersElasticSearch(usersDto);

            return Ok();
        }

        /// <summary>
        /// Searches in users.
        /// </summary>
        /// <param name="query">The pharse you want to match.</param>
        /// <param name="from">Start point of the search.</param>
        /// <param name="size">Number of results returned.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("")]
        public async Task<UserCompleteDto> GetUsersBySearch(string query, int from = 0, int size = 20)
        {
            var usersDto = await _elasticsearch.GetUsers(query, from, size);

            var result = new UserCompleteDto();
            result.Users = usersDto.ToList();
            return result;
        }

        

        /// <summary>
        /// To reset password.
        /// </summary>
        /// <param name="email">The email used for that account.</param>
        /// <returns></returns>
        [HttpGet]
        [Route("renew")]
        public async Task<IHttpActionResult> GetNewPassword(string email)
        {
           var password = System.Web.Security.Membership.GeneratePassword(8, 0);
           var salt = Encrypting.GenerateSalt();
           var hashedPassword = Encrypting.Hash(password, salt);

           var user = await _userRepository.GetSingleAsync(u => u.Email.Equals(email));
           if (user == null)
           {
               return NotFound();
           }
           var userCredentials = await _userCredentialsRepsoitory.GetSingleAsync(u => u.Username.Equals(user.Username));
           userCredentials.Password = hashedPassword;
           userCredentials.Salt = salt;
           await _userCredentialsRepsoitory.UpdateAsync(userCredentials);
            


            MailHelper.SendMail("New password for you dude", "This is your new password: " + password , user.Email);
            return Ok();
        }

    }
}