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
using MicrobrewitModel;
using MicrobrewitApi.DTOs;
using MicrobrewitApi.Util;
using log4net;
using System.Web;
using System.Security.Principal;

namespace MicrobrewitApi.Controllers
{
    [RoutePrefix("api/users")]
    public class UsersController : ApiController
    {
        private MicrobrewitApiContext db = new MicrobrewitApiContext();
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);            

        // GET api/User
        [Route("")]
        public IQueryable<User> GetUsers()
        {
            return db.Users;
        }

        // GET api/User/5
        [Route("{username}")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> GetUser(string username)
        {
            User user = await db.Users.FindAsync(username);
            if (user == null)
            {
                return NotFound();
            }

            return Ok(user);
        }

        // PUT api/User/5
        [Route("{id}")]
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

        [Route("login")]
        [AntiForgeryValidate]
        public async Task<IHttpActionResult> PostLogin(Login login)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var userCredentials = await db.UserCredentials.Include(u => u.User).Where(u => u.User.Username.Equals(login.UserName)).FirstOrDefaultAsync();

            if (!Encrypting.Decrypt(userCredentials.Password, userCredentials.SharedSecret).Equals(login.Password))
            {
                return BadRequest();
            }
            else
            {
                HttpContext.Current.User = new GenericPrincipal(new ApiIdentity(userCredentials.User), new string[] { });
            }

            return Ok(HttpContext.Current.User);
        }


        // POST api/User
        [Route("")]
        [ResponseType(typeof(User))]
        public async Task<IHttpActionResult> PostUser(UserDto userDto)
        {

            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var encryptedPassword = Encrypting.Encrypt(userDto.Password, userDto.SharedSecret);
            var token = "test";

            var user = new User()
            {
                Username = userDto.UserName,
                Email = userDto.Email,
                BreweryName = userDto.BreweryName,
                Settings = userDto.Settings,
            };
            UserCredentials userCredential = new UserCredentials() { Password = encryptedPassword, SharedSecret = userDto.SharedSecret, Token = token, Username = user.Username };


            db.UserCredentials.Add(userCredential);
            db.Users.Add(user);
            await db.SaveChangesAsync();
            return CreatedAtRoute("DefaultApi", new { controller = "users", id = user.Username }, user);
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