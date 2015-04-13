using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Repository
{
    public interface IUserRepository : IGenericDataRepository<User>
    {
        IEnumerable<UserSocial> GetUserSocials(string username);
        Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string username);
        Task<bool> ConfirmBreweryMemberAsync(string username, NotificationDto notificationDto);
        Task<bool> ConfirmUserBeerAsync(string username, NotificationDto notificationDto);
    }
}
