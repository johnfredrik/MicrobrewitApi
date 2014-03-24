using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class UserRepository : IUserRepository
    {

        public IList<User> GetUsers()
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Users.ToList();
            }
        }

        public User GetUser(string id)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.Users.Where(u => u.Username.Equals(id)).SingleOrDefault();
            }
        }
    }
}
