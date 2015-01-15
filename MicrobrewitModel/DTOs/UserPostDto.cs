using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using Newtonsoft.Json;

namespace Microbrewit.Model.DTOs
{
    public class UserPostDto
    {
        [Required]
        [Display(Name = "User name")]
        [JsonProperty(PropertyName = "username")]
        public string Username { get; set; }
        [Required]
        [StringLength(500, ErrorMessage = "The{0} must be at leaste {2} characters long", MinimumLength = 2)]
        [JsonProperty(PropertyName = "email")]
        public string Email { get; set; }
        [JsonProperty(PropertyName = "brewery")]
        public DTO Brewery { get; set; }
        [JsonProperty(PropertyName = "settings")]
        public string Settings { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "The {0} must be at least {2} characters long.", MinimumLength = 6)]
        [DataType(DataType.Password)]
        [Display(Name = "Password")]
        [JsonProperty(PropertyName = "password")]
        public string Password { get; set; }
        [DataType(DataType.Password)]
        [JsonProperty(PropertyName = "confirmPassword")]
        [Display(Name = "Confirm password")]
        [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
        public string ConfirmPassword { get; set; }
         [JsonProperty(PropertyName = "geoLocation")]
        public GeoLocationDto GeoLocation { get; set; }


        

        

        
    }
}