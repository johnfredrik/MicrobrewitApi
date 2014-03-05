using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel
{
    public class UserCredentials
    {
        public int Id { get; set; }
        public string Password { get; set; }
        public string PasswordSalt { get; set; }
        public User User { get; set; }       
    }
}
