using System;
using System.Configuration;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Reflection;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http;
using AutoMapper;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Interface;
using Microsoft.AspNet.Identity;
using Thinktecture.IdentityModel.Authorization;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("Account")]
    public class AccountController : ApiController
    {
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);
        private static readonly string MailService = ConfigurationManager.AppSettings["mailService"];
        private readonly string _blobPath = "https://microbrewit.blob.core.windows.net/images/";
        private readonly IUserService _userService;
        private AuthRepository _repo = null;

        public AccountController(IUserService userService)
        {
            _userService = userService;
            _repo = new AuthRepository();
        }

        // POST api/Account/Register
        /// <summary>
        /// Register user account to microbrew.it
        /// </summary>
        /// <param name="userPostDto"></param>
        /// <returns></returns>
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
            if (result.Succeeded)
            {
                var dbUser = await _repo.FindUser(userModel.UserName, userModel.Password);
                await _userService.UpdateAsync(Mapper.Map<User, UserDto>(user));

                var code = await _repo.GenerateEmailConfirmationTokenAsync(dbUser.Id);
                var callbackUrl = string.Format("{0}/account/confirm?userid={1}&code={2}", MailService, HttpUtility.UrlEncode(dbUser.Id),
                    HttpUtility.UrlEncode(code));

                try
                {
                    await _repo.SendEmailAsync(dbUser.Id,
                        "Confirm your account",
                        "Please confirm your account by clicking this link: <a href=\""
                        + callbackUrl + "\">link</a>");
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
            }

            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }
            return Ok(Mapper.Map<User, UserDto>(user));
        }

        /// <summary>
        /// To resend a confirm mail request.
        /// </summary>
        /// <param name="username"></param>
        /// <returns></returns>
        [Route("resend")]
        [HttpGet]
        public async Task<IHttpActionResult> ReSendConfirmationMail(string username)
        {
            Log.Debug("Auth");
            var isAllowed = ClaimsAuthorization.CheckAccess("Resend", "User", username);
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            var dbUser = await _repo.FindByNameAsync(username);
            if (dbUser != null)
            {
                var code = await _repo.GenerateEmailConfirmationTokenAsync(dbUser.Id);
                var callbackUrl = string.Format("{0}/account/confirm?userid={1}&code={2}", MailService, HttpUtility.UrlEncode(dbUser.Id),
                    HttpUtility.UrlEncode(code));

                try
                {
                    await _repo.SendEmailAsync(dbUser.Id,
                        "Confirm your account",
                        "Please confirm your account by clicking this link: <a href=\""
                        + callbackUrl + "\">link</a>");
                }
                catch (Exception ex)
                {
                    Log.Error(ex);
                }
                return Ok();
            }
            return BadRequest();
        }

        [Route("forgot")]
        [HttpPost]
        public async Task<IHttpActionResult> ForgotPassWord(ForgotPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repo.FindByEmailAsync(model.Email);
                if (user == null || !(await _repo.IsEmailConfirmedAsync(user.Id)))
                {
                    // Don't reveal that the user does not exist or is not confirmed
                    return Ok("Request sent");
                }

                var code = await _repo.GeneratePasswordResetTokenAsync(user.Id);
                var callbackUrl = string.Format("http://http://microbrew.it/account/resetpassword?id={0}&code={1}", HttpUtility.UrlEncode(user.Id), HttpUtility.UrlEncode(code));
                await _repo.SendEmailAsync(user.Id, "Reset Password",
                "<a href=\"" + callbackUrl + "\">link</a>");
                return Ok("Request sent");
            }
            return BadRequest("Invalid request");
        }

        [Route("reset")]
        [HttpPost]
        public async Task<IHttpActionResult> ResetPassword(ResetPassword model)
        {
            if (ModelState.IsValid)
            {
                var user = await _repo.FindByEmailAsync(model.Email);
                if (user == null || !(await _repo.IsEmailConfirmedAsync(user.Id)))
                {
                    return BadRequest();
                }
                var result = await _repo.ResetPasswordAsync(user.Id, model.Token, model.Password);
                if (result.Succeeded)
                {
                    return Ok();
                }
                IHttpActionResult errorResult = GetErrorResult(result);

                if (errorResult != null)
                {
                    return errorResult;
                }
            }
            return BadRequest();
        }

        // PUT api/User/5
        [Route("{username}")]
        public async Task<IHttpActionResult> PutUser(string username, UserPutDto userPutDto)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Put", "User", username);
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            if (!username.Equals(userPutDto.Username))
            {
                return BadRequest();
            }
            var userModel = Mapper.Map<UserPutDto, UserModel>(userPutDto);
            var result = await _repo.UpdateUser(userModel);
            if (result.Succeeded)
            {
                var user = Mapper.Map<UserPutDto, UserDto>(userPutDto);
                await _userService.UpdateAsync(user);
            }
            IHttpActionResult errorResult = GetErrorResult(result);
            if (errorResult != null)
            {
                return errorResult;
            }

            return StatusCode(HttpStatusCode.NoContent);
        }

        [Route("Confirm")]
        [HttpGet]
        public async Task<IHttpActionResult> ConfirmEmail(string userId, string code)
        {
           
            if (userId == null || code == null)
            {
                return BadRequest("wrong");
            }

            var result = await _repo.ConfirmEmailAsync(userId, code);
            if (result.Succeeded)
            {
                return Ok("ConfirmEmail");
            }
            IHttpActionResult errorResult = GetErrorResult(result);

            if (errorResult != null)
            {
                return errorResult;
            }
            return BadRequest();
        }

        [Route("{username}/upload")]
        [HttpPost]
        public async Task<IHttpActionResult> UploadFile(string username)
        {
            var isAllowed = ClaimsAuthorization.CheckAccess("Upload", "User", username);
            if (!isAllowed)
            {
                return StatusCode(HttpStatusCode.Unauthorized);
            }
            if (!Request.Content.IsMimeMultipartContent())
            {
                throw new HttpResponseException(HttpStatusCode.UnsupportedMediaType);
            }
            var userDto = await _userService.GetSingleAsync(username);
            if (userDto == null) return NotFound();
            MultipartStreamProvider provider = new BlobStorageMultipartStreamProvider(userDto);
            await Request.Content.ReadAsMultipartAsync(provider);
            var avatar = provider.Contents.SingleOrDefault(p => p.Headers.ContentDisposition.Name.Contains("avatar"));
            var headerImage =
                provider.Contents.SingleOrDefault(p => p.Headers.ContentDisposition.Name.Contains("headerImage"));
            if (avatar != null)
            {
                var fileName = avatar.Headers.ContentDisposition.FileName;
                userDto.Avatar = fileName;
            }
            if (headerImage != null)
            {
                var fileName = headerImage.Headers.ContentDisposition.FileName;
                userDto.HeaderImage = fileName;
            }
            await _userService.UpdateAsync(userDto);
            return Ok();
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
