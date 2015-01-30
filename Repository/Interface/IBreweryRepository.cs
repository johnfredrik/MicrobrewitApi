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
        Task<BreweryMember> GetSingleMemberAsync(int breweryId,string username);
        Task<IList<BreweryMember>> GetAllMembersAsync(int breweryId);
        Task DeleteMember(int breweryId, string username);
        Task UpdateMemberAsync(BreweryMember breweryMember);
        Task AddMemberAsync(BreweryMember breweryMember);
        IList<BreweryMember> GetMemberships(string username);
    }
}
