using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using log4net;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository.Interface;

namespace Microbrewit.Repository.Repository
{
    public class UserDapperRepository : IUserRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        private readonly IAuthRepository _authRepository = new AuthRepository();

        public IEnumerable<UserSocial> GetUserSocials(string username)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                return context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                    new {Username = username});
            }
        }

        public async Task<IEnumerable<UserBeer>> GetAllUserBeersAsync(string username)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                    "SELECT * " +
                    "FROM UserBeers ub " +
                    "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                    {
                        userBeer.Beer = beer;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        return userBeer;
                    }, new { Username = username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                return userBeers;
            }
        }

        public async Task<bool> ConfirmBreweryMemberAsync(string username, NotificationDto notificationDto)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    int result = 0;
                    try
                    {
                        if (notificationDto.Value)
                        {
                            result = await context.ExecuteAsync(
                                "Update BreweryMembers set Confirmed = @Confirmed WHERE MemberUsername = @Username and BreweryId = @BreweryId",
                                new {Username = username, BreweryId = notificationDto.Id, Confirmed = true}, transaction);

                        }
                        else
                        {
                            result = await context.ExecuteAsync("DELETE FROM BreweryMembers  WHERE MemberUsername = @Username and BreweryId = @BreweryId",
                                new { Username = username, BreweryId = notificationDto.Id}, transaction);
                        }
                        transaction.Commit();
                        return result > 0;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<bool> ConfirmUserBeerAsync(string username, NotificationDto notificationDto)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    int result = 0;
                    try
                    {
                        if (notificationDto.Value)
                        {
                          result = await context.ExecuteAsync("UPDATE UserBeers set Confirmed = @Confirmed WHERE Username = @Username and BeerId = @BeerId", 
                                new {Confirmed = true, Username = username, BeerId = notificationDto.Id},
                                transaction);
                        }
                        else
                        {
                           result = await context.ExecuteAsync("DELETE FROM UserBeers WHERE Username = @Username and BeerId = @BeerId;", new {Username = username, BeerId = notificationDto.Id},transaction);
                        } 
                        transaction.Commit();
                        return result > 0;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public IList<User> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var users = context.Query<User>("SELECT * FROM Users;");

                foreach (var user in users)
                {
                    var userSocials = context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                        new { user.Username });
                    user.Socials = userSocials.ToList();
                    var breweryMembers =
                        context.Query<BreweryMember, Brewery, BreweryMember>("SELECT * FROM BreweryMembers bm " +
                                                                             "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                                                                             "WHERE bm.MemberUsername = @Username",
                            (breweryMember, brewery) =>
                            {
                                breweryMember.Brewery = brewery;
                                return breweryMember;
                            },
                            new { user.Username }, splitOn: "BreweryId");
                    user.Breweries = breweryMembers.ToList();

                    var userBeers = context.Query<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                        "SELECT * " +
                        "FROM UserBeers ub " +
                        "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                        "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                        "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                        "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                        "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                        "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                        {
                            userBeer.Beer = beer;
                            if (srm != null)
                                beer.SRM = srm;
                            if (abv != null)
                                beer.ABV = abv;
                            if (ibu != null)
                                beer.IBU = ibu;
                            if (beerStyle != null)
                                beer.BeerStyle = beerStyle;
                            return userBeer;
                        }, new { user.Username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                    user.Beers = userBeers.ToList();
                }
               
                //foreach (var user in users)
                //{
                //    var account = await _authRepository.FindUser(user.Username);
                //    if (account == null || !account.Roles.Any()) continue;
                //        user.Roles = account.Roles.Select(r => r.RoleId);
                //}
                return users.ToList();
            }
        }

        public User GetSingle(string username, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var users = context.Query<User>("SELECT * FROM Users WHERE Username = @Username;", new { Username = username });
                var user = users.SingleOrDefault();
                if (user == null) return null;

                var userSocials = context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                    new { user.Username });
                user.Socials = userSocials.ToList();
                var breweryMembers =
                    context.Query<BreweryMember, Brewery, BreweryMember>(
                        "SELECT * FROM BreweryMembers bm " +
                        "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                        "WHERE bm.MemberUsername = @Username",
                        (breweryMember, brewery) =>
                        {
                            breweryMember.Brewery = brewery;
                            return breweryMember;
                        },
                        new { user.Username }, splitOn: "BreweryId");
                user.Breweries = breweryMembers.ToList();

                var userBeers = context.Query<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                    "SELECT * " +
                    "FROM UserBeers ub " +
                    "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                    {
                        userBeer.Beer = beer;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        return userBeer;
                    }, new { user.Username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                user.Beers = userBeers.ToList();

                return user;

            }
        }

        public void Add(User user)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute(
                            "INSERT Users(Username,Email,Settings,Gravatar,Latitude,Longitude,HeaderImage,Avatar) " +
                            "VALUES(@Username,@Email,@Settings,@Gravatar,@Latitude,@Longitude,@HeaderImage,@Avatar);",
                            user, transaction);
                        if (user.Socials != null)
                        {
                            context.Execute(
                                "INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                                user.Socials.Select(u => new { user.Username, u.Site, u.Url }), transaction);
                        }

                        var userSocials =
                            context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                               new { user.Username }, transaction);
                        user.Socials = userSocials.ToList();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public void Update(User user)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute(
                            "UPDATE Users set Username = @Username, Email = @Email, Settings = @Settings ,Gravatar = @Gravatar," +
                            " Latitude = @Latitude, Longitude = @Longitude, HeaderImage = @HeaderImage, Avatar = @Avatar " +
                            "WHERE Username = @Username;",
                            user, transaction);
                        UpdateUserSocials(context, transaction, user);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }


        //TODO: Need to define the logic around delete.
        public void Remove(User user)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute("DELETE FROM Users WHERE Username = @Username", new {user.Username},transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<IList<User>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                var users = await context.QueryAsync<User>("SELECT * FROM Users;");
                foreach (var user in users)
                {
                    var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                        new { user.Username });
                    user.Socials = userSocials.ToList();
                    var breweryMembers =
                        await context.QueryAsync<BreweryMember, Brewery, BreweryMember>("SELECT * FROM BreweryMembers bm " +
                                                                             "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                                                                             "WHERE bm.MemberUsername = @Username",
                            (breweryMember, brewery) =>
                            {
                                breweryMember.Brewery = brewery;
                                return breweryMember;
                            },
                            new { user.Username }, splitOn: "BreweryId");
                    user.Breweries = breweryMembers.ToList();

                    var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                        "SELECT * " +
                        "FROM UserBeers ub " +
                        "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                        "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                        "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                        "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                        "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                        "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                        {
                            userBeer.Beer = beer;
                            if (srm != null)
                                beer.SRM = srm;
                            if (abv != null)
                                beer.ABV = abv;
                            if (ibu != null)
                                beer.IBU = ibu;
                            if (beerStyle != null)
                                beer.BeerStyle = beerStyle;
                            return userBeer;
                        }, new { user.Username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                    user.Beers = userBeers.ToList();
                }
                foreach (var user in users)
                {
                    user.Roles = new List<string>();
                    var identityUser = await _authRepository.FindUser(user.Username);
                    if (identityUser != null && identityUser.Roles.Any())
                    {
                        if(identityUser.Roles.Any(r => r.RoleId == "1"))
                            user.Roles.Add("Admin");
                    }
                }

                return users.ToList();

            }
        }

        public async Task<User> GetSingleAsync(string username, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetConnection())
            {
                //Dapper returns IEnumerable, but result should always be single row.
                var users = await context.QueryAsync<User>("SELECT * FROM Users WHERE Username = @Username;", new { Username = username });
                var user = users.SingleOrDefault();
                if (user == null) return null;

                var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                    new { user.Username });
                user.Socials = userSocials.ToList();
                var breweryMembers =
                    context.Query<BreweryMember, Brewery, BreweryMember>(
                        "SELECT * FROM BreweryMembers bm " +
                        "LEFT JOIN Breweries b ON bm.BreweryId = b.BreweryId " +
                        "WHERE bm.MemberUsername = @Username",
                        (breweryMember, brewery) =>
                        {
                            breweryMember.Brewery = brewery;
                            return breweryMember;
                        },
                        new { user.Username }, splitOn: "BreweryId");
                user.Breweries = breweryMembers.ToList();

                var userBeers = await context.QueryAsync<UserBeer, Beer, SRM, ABV, IBU, BeerStyle, UserBeer>(
                    "SELECT * " +
                    "FROM UserBeers ub " +
                    "LEFT JOIN Beers b ON ub.BeerId = b.BeerId " +
                    "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                    "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                    "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                    "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                    "WHERE ub.Username = @Username", (userBeer, beer, srm, abv, ibu, beerStyle) =>
                    {
                        userBeer.Beer = beer;
                        if (srm != null)
                            beer.SRM = srm;
                        if (abv != null)
                            beer.ABV = abv;
                        if (ibu != null)
                            beer.IBU = ibu;
                        if (beerStyle != null)
                            beer.BeerStyle = beerStyle;
                        return userBeer;
                    }, new { user.Username }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                user.Beers = userBeers.ToList();
                user.Roles = new List<string>();
                var identityUser = await _authRepository.FindUser(user.Username);
                if (identityUser != null && identityUser.Roles.Any())
                {
                    if (identityUser.Roles.Any(r => r.RoleId == "1"))
                        user.Roles.Add("Admin");
                }
                return user;
            }
        }

        public async Task AddAsync(User user)
        {
            using (var context = DapperHelper.GetConnection())
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        await context.ExecuteAsync(
                            "INSERT Users(Username,Email,Settings,Gravatar,Latitude,Longitude,HeaderImage,Avatar) " +
                            "VALUES(@Username,@Email,@Settings,@Gravatar,@Latitude,@Longitude,@HeaderImage,@Avatar);",
                            user, transaction);
                        if (user.Socials != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                                user.Socials.Select(u => new { user.Username, u.Site, u.Url }), transaction);
                        }

                        var userSocials =
                            await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                               new { user.Username }, transaction);
                        user.Socials = userSocials.ToList();
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(User user)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                       var result = await context.ExecuteAsync(
                             "UPDATE Users set Username = @Username, Email = @Email, Settings = @Settings ,Gravatar = @Gravatar," +
                             " Latitude = @Latitude, Longitude = @Longitude, HeaderImage = @HeaderImage, Avatar = @Avatar " +
                             "WHERE Username = @Username;",
                             user, transaction);
                        await UpdateUserSocialsAsync(context, transaction, user);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                        throw;
                    }
                }
            }
        }

        public Task RemoveAsync(User user)
        {
            throw new NotImplementedException();
        }

        private void UpdateUserSocials(DbConnection context, DbTransaction transaction, User user)
        {
            var userSocials = context.Query<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                new { user.Username }, transaction);

            context.Execute("DELETE FROM UserSocials WHERE Username = @Username and SocialId = @SocialId;",
                userSocials.Where(
                    u => user.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            context.Execute(
                "UPDATE UserSocials set Site = @Site, Url = @Url WHERE Username = @Username and SocialId = @SocialId;",
                user.Socials, transaction);

            context.Execute("INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                user.Socials.Where(
                    s => userSocials.All(u => s.Username != u.Username && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
                transaction);
        }

        private async Task UpdateUserSocialsAsync(DbConnection context, DbTransaction transaction, User user)
        {
            var userSocials = await context.QueryAsync<UserSocial>("SELECT * FROM UserSocials WHERE Username = @Username",
                new { user.Username }, transaction);

            await context.ExecuteAsync("DELETE FROM UserSocials WHERE Username = @Username and SocialId = @SocialId;",
                userSocials.Where(
                    u => user.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            await context.ExecuteAsync(
                "UPDATE UserSocials set Site = @Site, Url = @Url WHERE Username = @Username and SocialId = @SocialId;",
                user.Socials, transaction);

            await context.ExecuteAsync("INSERT UserSocials(Username,Site,Url) VALUES(@Username,@Site,@Url);",
                user.Socials.Where(
                    s => userSocials.All(u => s.Username != u.Username && u.SocialId != s.SocialId)).Select(s => new { user.Username, s.Site, s.Url }),
                transaction);
        }
    }
}
