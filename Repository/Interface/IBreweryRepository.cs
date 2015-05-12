using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public interface IBreweryRepository
    {

        IList<Brewery> GetAll(params string[] navigationProperties);
        Brewery GetSingle(int id, params string[] navigationProperties);
        void Add(Brewery brewery);
        void Update(Brewery brewery);
        void Remove(Brewery brewery);

        //Async methods
        Task<IList<Brewery>> GetAllAsync(params string[] navigationProperties);
        Task<Brewery> GetSingleAsync(int id, params string[] navigtionProperties);
        Task AddAsync(Brewery brewery);
        Task<int> UpdateAsync(Brewery brewery);
        Task RemoveAsync(Brewery brewery);

        Task<BreweryMember> GetSingleMemberAsync(int breweryId,string username);
        Task<IList<BreweryMember>> GetAllMembersAsync(int breweryId);
        Task DeleteMember(int breweryId, string username);
        Task UpdateMemberAsync(BreweryMember breweryMember);
        Task AddMemberAsync(BreweryMember breweryMember);
        IList<BreweryMember> GetMemberships(string username);
        IList<BreweryMember> GetMembers(int breweryId);
        IList<BrewerySocial> GetBrewerySocials(int breweryId);



    }
}
