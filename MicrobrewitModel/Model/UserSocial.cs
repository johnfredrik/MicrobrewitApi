using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class UserSocial
    {
        public int SocialId { get; set; }
        public string Username { get; set; }
        public string Site { get; set; }
        public string Url { get; set; }
        public User User { get; set; }
    }
}
