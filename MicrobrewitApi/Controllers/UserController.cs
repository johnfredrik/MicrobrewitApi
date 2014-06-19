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

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IUserRepository _userRepository;
        private readonly IUserCredentialRepository userCredentialsRepsoitory = new UserCredentialRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public UserController(IUserRepository userRepository)
        {
            this._userRepository = userRepository;
        }

        // GET api/User
        [TokenValidation]
        [Route("")]
        public UserCompleteDto GetUsers()
        {
            var users = Mapper.Map<IList<User>, IList<UserDto>>(_userRepository.GetAll());
            var result = new UserCompleteDto();
            result.Users = users;
            return result;
        }

        // GET api/User/5
        [Route("{username}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string username)
        {
            var user = Mapper.Map<User, UserDto>(_userRepository.GetSingle( u => u.Username.Equals(username)));
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserCompleteDto() { Users = new List<UserDto>() };
            result.Users.Add(user);
            return Ok(result);
        }

        // PUT api/User/5
        [Route("")]
        public async Task<IHttpActionResult> PutUser(string id, User user)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!id.Equals(user.Username))
            {
                return BadRequest();
            }

            db.Entry(user).State = EntityState.Modified;

            try
            {
                await db.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!UserExists(id))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        /// <summary>
        /// Takes username and password in base 
        /// </summary>
        /// <returns></returns>
        [LoginValidation]
        [Route("login")]
        public IHttpActionResult PostLogin()
        {                 
            return Ok();
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
            var password = Encoding.UTF8.GetString(Convert.FromBase64String(userPostDto.Password));
            var encryptedPassword = Encrypting.Encrypt(password, userPostDto.Email);
            var salt = Encrypting.GenerateRandomBytes(256 / 8);
           
            var user = Mapper.Map<UserPostDto,User>(userPostDto);
            var userCredential = new UserCredentials() { Password = encryptedPassword, SharedSecret = userPostDto.Email, Username = user.Username, User = user };
            userCredentialsRepsoitory.Add(userCredential);

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

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                db.Dispose();
            }
            base.Dispose(disposing);
        }

        private bool UserExists(string id)
        {
            return db.Users.Count(e => e.Username.Equals(id)) > 0;
        }

    }
}