using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading.Tasks;
using System.Web.Http;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository.Repository;
using Microsoft.AspNet.Identity;
using Microbrewit.Repository;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private AuthRepository _repo = null;
        private IUserRepository _userRepository = null;

        public AccountController()
        {
            _repo = new AuthRepository();
            _userRepository = new UserRepository();

        }

        // POST api/Account/Register
        [AllowAnonymous]
        [Route("Register")]
        public async Task<IHttpActionResult> Register(UserPostDto userPostDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            var userModel = Mapper.Map<UserPostDto, UserModel>(userPostDto);
            var user = Mapper.Map<UserPostDto, User>(userPostDto);
            IdentityResult result = await _repo.RegisterUser(userModel);
            await _userRepository.AddAsync(user);
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }

            return Ok();
        }

        // PUT api/User/5
        [Route("{username}")]
        public async Task<IHttpActionResult> PutUser(string username, UserPutDto userPutDto)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!username.Equals(userPutDto.Username))
            {
                return BadRequest();
            }
            var userModel = Mapper.Map<UserPutDto, UserModel>(userPutDto);
            await _repo.UpdateUser(userModel);
            var user = Mapper.Map<UserPutDto, User>(userPutDto);
            
            await _userRepository.UpdateAsync(user);

            return StatusCode(HttpStatusCode.NoContent);
        }

        protected override void Dispose(bool disposing)
        {
            if (disposing)
            {
                _repo.Dispose();
            }

            base.Dispose(disposing);
        }

        private IHttpActionResult GetErrorResult(IdentityResult result)
        {
            if (result == null)
            {
                return InternalServerError();
            }

            if (!result.Succeeded)
            {
                if (result.Errors != null)
                {
                    foreach (string error in result.Errors)
                    {
                        ModelState.AddModelError("", error);
                    }
                }

                if (ModelState.IsValid)
                {
                    // No ModelState errors are available to send, so just return an empty BadRequest.
                    return BadRequest();
                }

                return BadRequest(ModelState);
            }

            return null;
        }
    }
}
