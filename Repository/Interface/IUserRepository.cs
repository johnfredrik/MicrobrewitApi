using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Repository
{
    public interface IUserRepository
    {
        IEnumerable<UserSocial> GetUserSocials(string username);
        Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string username);
        Task<bool> ConfirmBreweryMemberAsync(string username, NotificationDto notificationDto);
        Task<bool> ConfirmUserBeerAsync(string username, NotificationDto notificationDto);

        IList<User> GetAll(params string[] navigationProperties);
        User GetSingle(string username, params string[] navigationProperties);
        void Add(User user);
        void Update(User user);
        void Remove(User user);

        //Async methods
        Task<IList<User>> GetAllAsync(params string[] navigationProperties);
        Task<User> GetSingleAsync(string username, params string[] navigtionProperties);
        Task AddAsync(User user);
        Task<int> UpdateAsync(User user);
        Task RemoveAsync(User user);
      
    }
}
