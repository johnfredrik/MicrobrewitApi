using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IBreweryRepository : IGenericDataRepository<Brewery>
    {
        Task<BreweryMember> GetBreweryMember(int breweryId,string username);
        Task<IList<BreweryMember>> GetBreweryMembers(int breweryId);
        Task DeleteBreweryMember(int breweryId, string username);
        Task UpdateBreweryMember(BreweryMember breweryMember);
        Task PostBreweryMember(BreweryMember breweryMember);

        IList<BreweryMember> GetBreweryMemberships(string username);

    }
}
