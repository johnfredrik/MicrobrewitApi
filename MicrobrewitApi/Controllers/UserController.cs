using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Threading.Tasks;
using System.Web.Http;
using System.Web.Http.Description;
using log4net;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;
using Microsoft.Ajax.Utilities;
using Newtonsoft.Json;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;
       

        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public UserController(IUserService userService)
        {
            _userService = userService;
        }

        // GET api/User
        [Route("")]
        public async Task<UserCompleteDto> GetUsers()
        {
            var users = await _userService.GetAllAsync();
            var result = new UserCompleteDto {Users = users.ToList()};
            return result;
        }

        // GET api/User/5
        [Route("{username}")]
        [ResponseType(typeof(UserCompleteDto))]
        public async Task<IHttpActionResult> GetUser(string username)
        {
            var user = await _userService.GetSingleAsync(username);
            if (user == null)
            {
                return NotFound();
            }
            var result = new UserCompleteDto() { Users = new List<UserDto>() };
            result.Users.Add(user);
            return Ok(result);
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
            var usersDto = await _userService.SearchAsync(query, from, size);
            return new UserCompleteDto {Users = usersDto.ToList()};
        }

        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [ClaimsAuthorize("Reindex","User")]
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateUsersElasticSearch()
        {
            await _userService.ReIndexElasticSearch();
            return Ok();
        }

        [Route("{username}/notifications")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserNotifications(string username)
        {
            var notifications = await _userService.GetUserNotificationsAsync(username);
            return Ok(notifications);
        }

        [Route("{username}/notifications/{id}")]
        [HttpGet]
        public async Task<IHttpActionResult> GetUserNotifications(string username,int id)
        {
            var notifications = await _userService.GetUserNotificationsAsync(username);
            return Ok(notifications);
        }

        [Route("{username}/notifications")]
        [HttpPost]
        public async Task<IHttpActionResult> UpdateUserNoTifications(string username, NotificationDto notificationDto)
        {

            var changed = await _userService.UpdateNotification(username, notificationDto);
            if (!changed) return BadRequest();
            return Ok();
        }
    }
}