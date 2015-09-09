using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Interface
{
    public interface IUserService
    {
        Task<IEnumerable<UserDto>> GetAllAsync();
        Task<UserDto> GetSingleAsync(string username);
        Task<UserDto> AddAsync(User user);
        Task<UserDto> DeleteAsync(string username);
        Task UpdateAsync(UserDto userDto);
        Task<IEnumerable<UserDto>> SearchAsync(string query, int from, int size);
        Task<IEnumerable<NotificationDto>> GetUserNotificationsAsync(string username);         
        
        Task ReIndexElasticSearch();
        Task ReIndexBeerRelationElasticSearch(BeerDto beerDto);
        Task ReIndexBreweryRelationElasticSearch(BreweryDto breweryDto);
        Task ReIndexUserElasticSearch(string username);
        Task<bool> UpdateNotification(string username, NotificationDto notificationDto);
    }
}
