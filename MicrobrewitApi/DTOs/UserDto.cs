using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class UserDto
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string BreweryName { get; set; }
        public string Settings { get; set; }
        public string Password { get; set; }
        public string SharedSecret { get; set; }
    }
}