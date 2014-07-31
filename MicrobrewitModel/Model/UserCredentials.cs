using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public byte[] Password { get; set; }
        public byte[] Salt { get; set; }
        public string Username { get; set; }
        public User User { get; set; }       
    }
}
