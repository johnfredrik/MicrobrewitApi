using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using log4net;

namespace Microbrewit.Repository
{
    public class BreweryRepository : IBreweryRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Brewery> GetAll(params string[] navigationProperties)
        {
            List<Brewery> list;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Brewery> dbQuery = context.Set<Brewery>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Brewery>(navigationProperty);

                list = dbQuery
                    .AsNoTracking()
                    .ToList<Brewery>();
            }
            return list;
        }

        public Brewery GetSingle(int id, params string[] navigationProperties)
        {
            Brewery item = null;
            using (var context = new MicrobrewitContext())
            {
                IQueryable<Brewery> dbQuery = context.Set<Brewery>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                    dbQuery = dbQuery.Include<Brewery>(navigationProperty);

                item = dbQuery
                    .AsNoTracking() //Don't track any changes for the selected item
                    .FirstOrDefault(s => s.BreweryId == id); //Apply where clause
            }
            return item;
        }

        public void Add(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {
                brewery.Origin = null;
                brewery.CreatedDate = DateTime.Now;
                brewery.UpdatedDate = DateTime.Now;
                context.Entry(brewery).State = EntityState.Added;
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

        public virtual void Update(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {
                brewery.UpdatedDate = DateTime.Now;
                var originalBrewery =
                    context.Breweries.Include(b => b.Members).SingleOrDefault(b => b.BreweryId == brewery.BreweryId);
                if (originalBrewery == null) return;
                brewery.CreatedDate = originalBrewery.CreatedDate;
                SetChanges(context, originalBrewery, brewery);
                foreach (var member in brewery.Members)
                {
                    var existingMember = originalBrewery.Members.Any(m => m.MemberUsername.Equals(member.MemberUsername));
                    if (existingMember)
                    {
                        var originalMember =
                            originalBrewery.Members.SingleOrDefault(m => m.MemberUsername.Equals(member.MemberUsername));
                        SetChanges(context, originalMember, member);
                    }
                    else
                    {
                        context.BreweryMembers.Add(member);
                    }
                }
                foreach (var brewerySocial in brewery.Socials)
                {
                    var existingBrewerySocial =
                        context.BrewerySocials.SingleOrDefault(
                            s => s.BreweryId == brewerySocial.BreweryId && s.SocialId == brewerySocial.SocialId);
                    if (existingBrewerySocial != null)
                    {
                        SetChanges(context, existingBrewerySocial, brewerySocial);
                    }
                    else
                    {
                        context.BrewerySocials.Add(brewerySocial);
                    }
                }
                brewery.Origin = null;


                try
                {
                    context.SaveChanges();
                }
                catch (Exception e)
                {

                    throw;
                }
            }
        }

        public virtual void Remove(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(brewery).State = EntityState.Deleted;
                context.SaveChanges();
            }
        }

        public virtual async Task<IList<Brewery>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Brewery> dbQuery = context.Set<Brewery>();

                //Apply eager loading
                foreach (string navigationProperty in navigationProperties)
                {
                    dbQuery = dbQuery.Include<Brewery>(navigationProperty);
                }
                return await dbQuery.ToListAsync();
            }
        }

        public virtual async Task<Brewery> GetSingleAsync(int id, params string[] navigationProperties)
        {
            using (var context = new MicrobrewitContext())
            {

                IQueryable<Brewery> dbQuery = context.Set<Brewery>();

                //Apply eager loading
                dbQuery = navigationProperties.Aggregate(dbQuery, (current, navigationProperty) => current.Include<Brewery>(navigationProperty));

                return await dbQuery.SingleOrDefaultAsync(s => s.BreweryId == id);
            }
        }

        public virtual async Task AddAsync(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(brewery).State = EntityState.Added;
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

        public virtual async Task<int> UpdateAsync(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {
                brewery.UpdatedDate = DateTime.Now;
                var originalBrewery =
                    context.Breweries.Include(b => b.Members).SingleOrDefault(b => b.BreweryId == brewery.BreweryId);
                if (originalBrewery == null) return -1;
                brewery.CreatedDate = originalBrewery.CreatedDate;
                SetChanges(context, originalBrewery, brewery);
                foreach (var member in brewery.Members)
                {
                    var existingMember = originalBrewery.Members.Any(m => m.MemberUsername.Equals(member.MemberUsername));
                    if (existingMember)
                    {
                        var originalMember =
                            originalBrewery.Members.SingleOrDefault(m => m.MemberUsername.Equals(member.MemberUsername));
                        SetChanges(context, originalMember, member);
                    }
                    else
                    {
                        context.BreweryMembers.Add(member);
                    }
                }
                foreach (var brewerySocial in brewery.Socials)
                {
                    var existingBrewerySocial =
                        context.BrewerySocials.SingleOrDefault(
                            s => s.BreweryId == brewerySocial.BreweryId && s.SocialId == brewerySocial.SocialId);
                    if (existingBrewerySocial != null)
                    {
                        SetChanges(context, existingBrewerySocial, brewerySocial);
                    }
                    else
                    {
                        context.BrewerySocials.Add(brewerySocial);
                    }
                }
                brewery.Origin = null;
                return await context.SaveChangesAsync();
            }
        }

        public virtual async Task RemoveAsync(Brewery brewery)
        {
            using (var context = new MicrobrewitContext())
            {

                context.Entry(brewery).State = EntityState.Deleted;
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

        public async Task<BreweryMember> GetSingleMemberAsync(int breweryId, string username)
        {
            using (var context = new MicrobrewitContext())
            {
                return await context.BreweryMembers.SingleOrDefaultAsync(bm => bm.MemberUsername.Equals(username) && bm.BreweryId == breweryId);
            }
        }

        public async Task DeleteMember(int breweryId, string username)
        {
            using (var context = new MicrobrewitContext())
            {
                var breweryMember = await context.BreweryMembers.SingleOrDefaultAsync(bm => bm.MemberUsername.Equals(username) && bm.BreweryId == breweryId);
                context.BreweryMembers.Remove(breweryMember);
                await context.SaveChangesAsync();
            }

        }

        public async Task<IList<BreweryMember>> GetAllMembersAsync(int breweryId)
        {
            using (var context = new MicrobrewitContext())
            {
                return await context.BreweryMembers.Where(b => b.BreweryId == breweryId).ToListAsync();
            }
        }

        public async Task UpdateMemberAsync(BreweryMember breweryMember)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(breweryMember).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task AddMemberAsync(BreweryMember breweryMember)
        {
            using (var context = new MicrobrewitContext())
            {
                context.BreweryMembers.Add(breweryMember);
                await context.SaveChangesAsync();
            }
        }

        public IList<BreweryMember> GetMemberships(string username)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.BreweryMembers.Where(b => b.MemberUsername.Equals(username)).ToList();
            }
        }

        public IList<BreweryMember> GetMembers(int breweryId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.BreweryMembers.Where(bm => bm.BreweryId == breweryId).ToList();
            }
        }

        public IList<BrewerySocial> GetBrewerySocials(int breweryId)
        {
            using (var context = new MicrobrewitContext())
            {
                return context.BrewerySocials.Where(b => b.BreweryId == breweryId).ToList();
            }
        }
    }
}
