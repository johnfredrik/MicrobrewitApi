using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MicrobrewitModel
{
    public class User
    {
        public int Id { get; set; }
        public int Username { get; set; }        
        public string Email { get; set; }
        public string BreweryName { get; set; }
        public string Settings { get; set; }
        public int UserCredentialsId { get; set; }
        public UserCredentials UserCredentials { get; set; }

    }
}
