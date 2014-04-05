using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class UserCompleteDto
    {
        public Meta Meta { get; set; }
        public IList<UserDto> Users { get; set; }
    }
}