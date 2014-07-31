using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class UserPostDto
    {
        public string Username { get; set; }
        public string Email { get; set; }
        public DTO Brewery { get; set; }
        public string Settings { get; set; }
        public string Password { get; set; }
    }
}