﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Api.DTOs
{
    public class UserDto
    {
        public string Id { get; set; }
        public string Href { get { return "http://api.microbrew.it/users/:username";} }
        public string Username { get; set; }
        public string Email { get; set; }
        public string BreweryName { get; set; }
        public string Settings { get; set; }
    }
}