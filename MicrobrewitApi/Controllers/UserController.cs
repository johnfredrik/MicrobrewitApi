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
        [Route("")]
        public async Task<UserCompleteDto> GetUsers(int from = 0, int size = 1000)
        {
            var users = await _elasticsearch.GetUsersAsync(from,size);
            if(!users.Any())
            {
                var temp = _userRepository.GetAll("Breweries.Brewery","Beers.Beer");
                users = Mapper.Map<IList<User>, IList<UserDto>>(temp);
            }
            var result = new UserCompleteDto();
            result.Users = users.ToList();
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
            var usersDto = await _elasticsearch.SearchUsers(query, from, size);

            var result = new UserCompleteDto();
            result.Users = usersDto.ToList();
            return result;
        }

        /// <summary>
        /// Updates elasticsearch with data from the database.
        /// </summary>
        /// <returns>200 OK</returns>
        [Route("es")]
        [HttpGet]
        public async Task<IHttpActionResult> UpdateUsersElasticSearch()
        {
            var users = await _userRepository.GetAllAsync();
            var userDto = Mapper.Map<IList<User>, IList<UserDto>>(users);
            // updated elasticsearch.
            await _elasticsearch.UpdateUsersElasticSearch(userDto);

            return Ok();
        }
    }
}