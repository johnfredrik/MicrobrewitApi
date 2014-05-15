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
        public override async Task<int> UpdateAsync(params Brewery[] breweries)
        {
            using (var context = new MicrobrewitContext())
            {
                foreach (var brewery in breweries)
                {
                    var originalBrewery = context.Breweries.Include(b => b.Members).SingleOrDefault(b => b.Id == brewery.Id);
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
                }
                return await context.SaveChangesAsync();
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


        public async Task<BreweryMember> GetBreweryMember(int breweryId, string username)
        {
            using (var context = new MicrobrewitContext())
            {
                return await context.BreweryMembers.SingleOrDefaultAsync(bm => bm.MemberUsername.Equals(username) && bm.BreweryId == breweryId);
            }
        }

        public async Task DeleteBreweryMember(int breweryId, string username)
        {
            using (var context = new MicrobrewitContext())
            {
               var breweryMember = await context.BreweryMembers.SingleOrDefaultAsync(bm => bm.MemberUsername.Equals(username) && bm.BreweryId == breweryId);
               context.BreweryMembers.Remove(breweryMember);
               await context.SaveChangesAsync();
            }
            
        }

        public async Task<IList<BreweryMember>> GetBreweryMembers(int breweryId)
        {
            using (var context = new MicrobrewitContext())
            {
                return await context.BreweryMembers.Where(b => b.BreweryId == breweryId).ToListAsync();
            }
        }


        public async Task UpdateBreweryMember(BreweryMember breweryMember)
        {
            using (var context = new MicrobrewitContext())
            {
                context.Entry(breweryMember).State = EntityState.Modified;
                await context.SaveChangesAsync();
            }
        }

        public async Task PostBreweryMember(BreweryMember breweryMember)
        {
            using (var context = new MicrobrewitContext())
            {
                context.BreweryMembers.Add(breweryMember);
                await context.SaveChangesAsync();
            }
        }
    }
}
