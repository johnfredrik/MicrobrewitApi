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
using Microbrewit.Api.DTOs;
using Microbrewit.Api.Util;
using Microbrewit.Repository;
using log4net;
using System.Web;
using System.Security.Principal;
using AutoMapper;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private MicrobrewitContext db = new MicrobrewitContext();
        private IUserRepository userRepository = new UserRepository();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);            

        // GET api/User
        [TokenValidation]
        [Route("")]
        public UserCompleteDto GetUsers()
        {
            var users = Mapper.Map<IList<User>, IList<UserDto>>(userRepository.GetUsers());
            var meta = new Meta();
            var result = new UserCompleteDto();
            
            meta.Returned = users.Count();
            result.Users = users;
            result.Meta = meta;
            return result;
        }

        // GET api/User/5
        [HttpGet]
        [Route("{username}")]
        [ResponseType(typeof(User))]
        public IHttpActionResult GetUser(string username)
        {
            var user = Mapper.Map<User, UserDto>(userRepository.GetUser(username));
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserCompleteDto() { Users = new List<UserDto>() };
            var meta = new Meta() { Returned = 1 };
            result.Users.Add(user);
            result.Meta = meta;
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

        [LoginValidation]
        [Route("login")]
        [HttpPost]
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
        [ResponseType(typeof(UserCompleteDto))]
        public async Task<IHttpActionResult> PostUser(UserPostDto userPostDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var encryptedPassword = Encrypting.Encrypt(userPostDto.Password, userPostDto.Email);
            var salt = Encrypting.GenerateRandomBytes(256 / 8);
           

            var user = new User()
            {
                Username = userPostDto.UserName,
                Email = userPostDto.Email,
                Settings = userPostDto.Settings,
            };
            
            UserCredentials userCredential = new UserCredentials() { Password = encryptedPassword, SharedSecret = userPostDto.Email, Username = user.Username };

            db.UserCredentials.Add(userCredential);
            db.Users.Add(user);
            await db.SaveChangesAsync();

            var result = new UserCompleteDto() { Users = new List<UserDto>()};
            var meta = new Meta() { Message = "The user was successfully added", Returned = 1 };
            result.Users.Add(Mapper.Map<User,UserDto>(user));
            result.Meta = meta;

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