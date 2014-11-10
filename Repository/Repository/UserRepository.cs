using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Security.Cryptography;

namespace Microbrewit.Repository
{
    public class UserRepository : GenericDataRepository<User>, IUserRepository
    {
        public override void Add(params User[] users)
        {
            foreach (var user in users)
            {
                var hash = HashEmailForGravatar(user.Email);
                user.Gravatar = string.Format(@"http://www.gravatar.com/avatar/{0}",hash);
            }
            base.Add(users);
        }

        /// Hashes an email with MD5.  Suitable for use with Gravatar profile
        /// image urls
        private static string HashEmailForGravatar(string email)
        {
            MD5 md5Hasher = MD5.Create();

            // Convert the input string to a byte array and compute the hash.  
            byte[] data = md5Hasher.ComputeHash(Encoding.Default.GetBytes(email));

            StringBuilder sBuilder = new StringBuilder();

            for (int i = 0; i < data.Length; i++)
            {
                sBuilder.Append(data[i].ToString("x2"));
            }

            return sBuilder.ToString();
        }
    }
}
