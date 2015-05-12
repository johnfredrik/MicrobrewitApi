using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using System.Security.Cryptography;
using log4net;
using Microbrewit.Model.DTOs;
using Microbrewit.Model.ModelBuilder;

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

        public IEnumerable<UserSocial> GetUserSocials(string username)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.UserSocials.Where(s => s.Username == username).ToList();
            }
        }

        public async Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string username)
        {
            using (var context = new MicrobrewitContext())
            {
                return await context.UserBeers.Where(u => u.Username == username).ToListAsync();
            }
        }

        public async Task<bool> ConfirmBreweryMemberAsync(string username, NotificationDto notificationDto)
        {
            using (var context = new MicrobrewitContext())
            {
                var breweryMember = await
                            context.BreweryMembers.SingleOrDefaultAsync(
                                b => b.BreweryId == notificationDto.Id && b.MemberUsername == username);
                if (breweryMember == null) return false;
                if (notificationDto.Value)
                {
                    breweryMember.Confirmed = true;
                }
                else
                {
                    context.BreweryMembers.Remove(breweryMember);
                }
                await context.SaveChangesAsync();
                return true;
            }
        }

        public async Task<bool> ConfirmUserBeerAsync(string username, NotificationDto notificationDto)
        {
            using (var context = new MicrobrewitContext())
            {
                var userBeer = await context.UserBeers.SingleOrDefaultAsync(b => b.BeerId == notificationDto.Id && b.Username == username);
                if (userBeer == null) return false;
                if (notificationDto.Value)
                {
                    userBeer.Confirmed = true;
                }
                else
                {
                    context.UserBeers.Remove(userBeer);
                }
                await context.SaveChangesAsync();
                return true;
            }
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

        public override async Task<int> UpdateAsync(params User[] users)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var user in users)
                {
                    var originalUser = context.Users.SingleOrDefault(u => u.Username == user.Username);
                    if (originalUser != null)
                    {
                        SetChanges(context,originalUser,user);
                        foreach (var social in user.Socials)
                        {
                            var originalSocial =
                                context.UserSocials.SingleOrDefault(
                                    s => s.Username == user.Username && s.SocialId == social.SocialId);
                            if (originalSocial != null)
                            {
                                SetChanges(context, originalSocial, social);
                            }
                            else
                            {
                                context.UserSocials.Add(social);
                            }
                        }
                    }
                }
                return await context.SaveChangesAsync();
            }
        }

        private static void SetChanges(MicrobrewitContext context, object original, object updated)
        {
            foreach (PropertyInfo propertyInfo in original.GetType().GetProperties())
            {
                if (propertyInfo.GetValue(updated, null) == null)
                {
                    propertyInfo.SetValue(updated, propertyInfo.GetValue(original, null), null);
                }
            }
            context.Entry(original).CurrentValues.SetValues(updated);
        }

        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<User> GetAll(params string[] navigationProperties)
        {
            List<User> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<User> dbQuery = context.Set<User>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<User>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<User>();
            }
            return list;
        }

        public User GetSingle(string username, params string[] navigationProperties)
        {
            User item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<User> dbQuery = context.Set<User>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<User>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.Username == username); //Apply where clause
            }
            return item;
        }

        public void Add(User user)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(user).State = EntityState.Added;
                try
                {
                    context.SaveChanges();
                }
                catch (DbEntityValidationException dbEx)
                {
                    foreach (var validationErrors in dbEx.EntityValidationErrors)
                    {
                        foreach (var validationError in validationErrors.ValidationErrors)
                        {
                            Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                            Log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                        }
                    }
                }
            }
        }

        public virtual void Update(User user)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(user).State = EntityState.Modified;
                context.SaveChanges();
            }
        }

        public virtual void Remove(User user)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(user).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<User>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<User> dbQuery = context.Set<User>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<User>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<User> GetSingleAsync(string username, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<User> dbQuery = context.Set<User>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<User>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.Username == username);
            }
        }

        public virtual async Task AddAsync(User user)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(user).State = EntityState.Added;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (DbEntityValidationException dbEx)
                {
                    //foreach (var validationErrors in dbEx.EntityValidationErrors)
                    //{
                    //    foreach (var validationError in validationErrors.ValidationErrors)
                    //    {
                    //        Trace.TraceInformation("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //        Log.DebugFormat("Property: {0} Error: {1}", validationError.PropertyName, validationError.ErrorMessage);
                    //        throw dbEx;
                    //    }
                    //}
                    throw;
                }
                catch (Exception ex)
                {
                    throw;
                }
            }

        }

        public virtual async Task<int> UpdateAsync(User user)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(user).State = EntityState.Modified;
                try
                {
                    return await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    throw;
                }

            }
        }

        public virtual async Task RemoveAsync(User user)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(user).State = EntityState.Deleted;
                try
                {
                    await context.SaveChangesAsync();
                }
                catch (Exception e)
                {
                    Log.Debug(e);
                    throw;
                }
            }
        }

    }
}
