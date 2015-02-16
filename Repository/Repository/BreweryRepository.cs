using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public class BreweryRepository : GenericDataRepository<Brewery>, IBreweryRepository
    {
        public Task AddAsync(params Brewery[] breweries)
        {

            foreach (var brewery in breweries)
            {
                brewery.Origin = null;
                brewery.CreatedDate = DateTime.Now;
                brewery.UpdatedDate = DateTime.Now;
            }
            return base.AddAsync(breweries);
        }

        public override async Task<int> UpdateAsync(params Brewery[] breweries)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var brewery in breweries)
                {
                    brewery.UpdatedDate = DateTime.Now;
                    var originalBrewery = context.Breweries.Include(b => b.Members).SingleOrDefault(b => b.Id == brewery.Id);
                    brewery.CreatedDate = originalBrewery.CreatedDate;
                    SetChanges(context, originalBrewery, brewery);
                    foreach (var member in brewery.Members)
                    {
                        var existingMember = originalBrewery.Members.Any(m => m.MemberUsername.Equals(member.MemberUsername));
                        if (existingMember)
                        {
                            var originalMember = originalBrewery.Members.SingleOrDefault(m => m.MemberUsername.Equals(member.MemberUsername));
                            SetChanges(context, originalMember, member);
                        }
                        else
                        {
                            context.BreweryMembers.Add(member);
                        }
                    }
                    foreach (var brewerySocial in brewery.Socials)
                    {
                        var existingBrewerySocial = context.BrewerySocials.SingleOrDefault(s => s.BreweryId == brewerySocial.BreweryId && s.SocialId == brewerySocial.SocialId);
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
                }

                try
                {
                    return await context.SaveChangesAsync();
                }
                catch (Exception e)
                {

                    throw;
                }
                //await base.UpdateAsync(breweries);

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
