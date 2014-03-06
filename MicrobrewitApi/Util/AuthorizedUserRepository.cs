using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using MicrobrewitModel;

namespace MicrobrewitApi.Util
{
    public class AuthorizedUserRepository
    {
        public static IList<UserCredentials> GetUsers()
        {
            using (var context = new MicrobrewitApiContext())
            {
                return context.UserCredentials.ToList();
               
            }
        }
    }
}