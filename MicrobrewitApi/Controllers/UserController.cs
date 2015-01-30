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
using Microbrewit.Service.Elasticsearch;
using Microbrewit.Service.Interface;
using Thinktecture.IdentityModel.Authorization.WebApi;

namespace Microbrewit.Api.Controllers
{
    [RoutePrefix("users")]
    public class UserController : ApiController
    {
        private readonly IUserService _userService;

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

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
        [ResponseType(typeof(User))]
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
            var result = new UserCompleteDto {Users = usersDto.ToList()};
            return result;
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
    }
}