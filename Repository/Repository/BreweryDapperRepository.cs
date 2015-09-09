using System;
using System.Collections.Generic;
using System.Data.Common;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using log4net;
using Microbrewit.Model;

namespace Microbrewit.Repository.Repository
{
    public class BreweryDapperRepository : IBreweryRepository
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        public IList<Brewery> GetAll(params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var breweries = context.Query<Brewery, Origin, Brewery>(
                    "SELECT * FROM Breweries b " +
                    "LEFT JOIN Origins o ON b.OriginId = o.OriginId;", (brewery, origin) =>
                    {
                        if (origin != null)
                            brewery.Origin = origin;
                        return brewery;
                    }, splitOn: "OriginId");

                foreach (var brewery in breweries)
                {
                    var breweryMembers =
                       context.Query<BreweryMember>(
                           "SELECT * FROM BreweryMembers bm " +
                           "WHERE bm.BreweryId = @BreweryId;",
                           new { brewery.BreweryId });
                    brewery.Members = breweryMembers.ToList();

                    var breweryBeers = context.Query<BreweryBeer, Beer, SRM, ABV, IBU, BeerStyle, BreweryBeer>(
                       "SELECT * " +
                       "FROM BreweryBeers bb " +
                       "LEFT JOIN Beers b ON bb.BeerId = b.BeerId " +
                       "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                       "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                       "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                       "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                       "WHERE bb.BreweryId = @BreweryId", (breweryBeer, beer, srm, abv, ibu, beerStyle) =>
                       {
                           breweryBeer.Beer = beer;
                           if (srm != null)
                               beer.SRM = srm;
                           if (abv != null)
                               beer.ABV = abv;
                           if (ibu != null)
                               beer.IBU = ibu;
                           if (beerStyle != null)
                               beer.BeerStyle = beerStyle;
                           return breweryBeer;
                       }, new { brewery.BreweryId }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                    brewery.Beers = breweryBeers.ToList();

                    var brewerySocials =
                        context.Query<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                            new { brewery.BreweryId });
                    brewery.Socials = brewerySocials.ToList();
                }
                return breweries.ToList();
            }
        }

        public Brewery GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var breweries = context.Query<Brewery, Origin, Brewery>(
                    "SELECT * FROM Breweries b " +
                    "LEFT JOIN Origins o ON b.OriginId = o.OriginId " +
                    "WHERE BreweryId = @BreweryId;", (b, origin) =>
                    {
                        if (origin != null)
                            b.Origin = origin;
                        return b;
                    }, new { BreweryId = id }, splitOn: "OriginId");

                var brewery = breweries.SingleOrDefault();
                var breweryMembers =
                   context.Query<BreweryMember>(
                       "SELECT * FROM BreweryMembers bm " +
                       "WHERE bm.BreweryId = @BreweryId;",
                       new { brewery.BreweryId });
                brewery.Members = breweryMembers.ToList();

                var breweryBeers = context.Query<BreweryBeer, Beer, SRM, ABV, IBU, BeerStyle, BreweryBeer>(
                   "SELECT * " +
                   "FROM BreweryBeers bb " +
                   "LEFT JOIN Beers b ON bb.BeerId = b.BeerId " +
                   "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                   "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                   "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                   "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                   "WHERE bb.BreweryId = @BreweryId", (breweryBeer, beer, srm, abv, ibu, beerStyle) =>
                   {
                       breweryBeer.Beer = beer;
                       if (srm != null)
                           beer.SRM = srm;
                       if (abv != null)
                           beer.ABV = abv;
                       if (ibu != null)
                           beer.IBU = ibu;
                       if (beerStyle != null)
                           beer.BeerStyle = beerStyle;
                       return breweryBeer;
                   }, new { brewery.BreweryId }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                brewery.Beers = breweryBeers.ToList();

                var brewerySocials =
                    context.Query<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                        new { brewery.BreweryId });
                brewery.Socials = brewerySocials.ToList();
                return brewery;
            }
        }

        public void Add(Brewery brewery)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        brewery.UpdatedDate = DateTime.Now;
                        brewery.CreatedDate = DateTime.Now;
                        var breweryId =
                            context.Query<int>(
                                "INSERT Breweries(Name,Description,Type,CreatedDate,UpdatedDate,Longitude,Latitude,Website,Established,HeaderImage,Avatar,OriginId,Address) " +
                                "VALUES (@Name,@Description,@Type,@CreatedDate,@UpdatedDate,@Longitude,@Latitude,@Website,@Established,@HeaderImage,@Avatar,@OriginId,@Address); " +
                                "SELECT CAST(SCOPE_IDENTITY() as int);", brewery, transaction);
                        brewery.BreweryId = breweryId.SingleOrDefault();
                        if (brewery.Socials != null)
                        {
                            context.Execute(
                                "INSERT BrewerySocials(BreweryId,Site,Url) VALUES(@BreweryId,@Site,@Url);",
                                brewery.Socials.Select(u => new { brewery.BreweryId, u.Site, u.Url }), transaction);
                            var brewerySocials =
                           context.Query<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                              new { brewery.BreweryId }, transaction);
                            brewery.Socials = brewerySocials.ToList();

                        }
                        if (brewery.Members != null)
                        {
                            context.Execute(
                                "INSERT BreweryMembers(BreweryId,MemberUsername,Role) VALUES(@BreweryId,@MemberUsername,@Role);",
                                brewery.Members.Select(u => new { brewery.BreweryId, u.MemberUsername, u.Role }), transaction);
                            brewery.Members = brewery.Members.Select(m => new BreweryMember { BreweryId = brewery.BreweryId, MemberUsername = m.MemberUsername, Role = m.Role, Confirmed = m.Confirmed }).ToList();
                        }
                        if (brewery.Beers != null)
                        {
                            context.Execute(
                                "INSERT BreweryBeers(BeerId,BreweryId) VALUES(@BeerId,@BreweryId);",
                                brewery.Beers.Select(b => new { b.BeerId, brewery.BreweryId }), transaction);
                            brewery.Beers = brewery.Beers.Select(m => new BreweryBeer { BreweryId = brewery.BreweryId, BeerId = m.BeerId }).ToList();
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Commit();
                        throw;
                    }
                }
            }
        }

        public void Update(Brewery brewery)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        context.Execute(
                            "UPDATE Breweries set Name = @Name,Description = @Name, Type = @Type, CreatedDate = @CreatedDate, UpdatedDate = @UpdatedDate," +
                            "Longitude = @Longitude, Latitude = @Latitude, Website = @Website, Established = @Established, HeaderImage = @HeaderImage, Avatar = @Avatar," +
                            "OriginId = @OriginId, Address = @Address WHERE BreweryId = @BreweryId;", brewery,
                            transaction);
                        UpdateBrewerySocials(context, transaction, brewery);
                        UpdateBreweryMembers(context, transaction, brewery);
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

        public void Remove(Brewery brewery)
        {
            throw new NotImplementedException();
        }

        public async Task<IList<Brewery>> GetAllAsync(int from, int size, params string[] navigationProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var breweries = await context.QueryAsync<Brewery, Origin, Brewery>(
                    "SELECT * FROM Breweries b " +
                    "LEFT JOIN Origins o ON b.OriginId = o.OriginId " +
                    "ORDER By BreweryId " +
                    "OFFSET @From ROWS FETCH NEXT @Size ROWS ONLY;", (brewery, origin) =>
                    {
                        if (origin != null)
                            brewery.Origin = origin;
                        return brewery;
                    }, new { From = from, Size = size }, splitOn: "OriginId");

                foreach (var brewery in breweries)
                {
                    var breweryMembers =
                       context.Query<BreweryMember>(
                           "SELECT * FROM BreweryMembers bm " +
                           "WHERE bm.BreweryId = @BreweryId;",
                           new { brewery.BreweryId });
                    brewery.Members = breweryMembers.ToList();

                    var breweryBeers = await context.QueryAsync<BreweryBeer, Beer, SRM, ABV, IBU, BeerStyle, BreweryBeer>(
                       "SELECT * " +
                       "FROM BreweryBeers bb " +
                       "LEFT JOIN Beers b ON bb.BeerId = b.BeerId " +
                       "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                       "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                       "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                       "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                       "WHERE bb.BreweryId = @BreweryId", (breweryBeer, beer, srm, abv, ibu, beerStyle) =>
                       {
                           breweryBeer.Beer = beer;
                           if (srm != null)
                               beer.SRM = srm;
                           if (abv != null)
                               beer.ABV = abv;
                           if (ibu != null)
                               beer.IBU = ibu;
                           if (beerStyle != null)
                               beer.BeerStyle = beerStyle;
                           return breweryBeer;
                       }, new { brewery.BreweryId }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                    brewery.Beers = breweryBeers.ToList();

                    var brewerySocials =
                        await context.QueryAsync<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                            new { brewery.BreweryId });
                    brewery.Socials = brewerySocials.ToList();
                }
                return breweries.ToList();
            }
        }

        public async Task<Brewery> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var breweries = await context.QueryAsync<Brewery, Origin, Brewery>(
                    "SELECT * FROM Breweries b " +
                    "LEFT JOIN Origins o ON b.OriginId = o.OriginId " +
                    "WHERE BreweryId = @BreweryId;", (b, origin) =>
                    {
                        if (origin != null)
                            b.Origin = origin;
                        return b;
                    }, new { BreweryId = id }, splitOn: "OriginId");

                var brewery = breweries.SingleOrDefault();
                var breweryMembers =
                   await context.QueryAsync<BreweryMember>(
                       "SELECT * FROM BreweryMembers bm " +
                       "WHERE bm.BreweryId = @BreweryId;",
                       new { brewery.BreweryId });
                brewery.Members = breweryMembers.ToList();

                var breweryBeers = await context.QueryAsync<BreweryBeer, Beer, SRM, ABV, IBU, BeerStyle, BreweryBeer>(
                   "SELECT * " +
                   "FROM BreweryBeers bb " +
                   "LEFT JOIN Beers b ON bb.BeerId = b.BeerId " +
                   "LEFT JOIN SRMs s ON s.SrmId = b.BeerId " +
                   "LEFT JOIN ABVs a ON a.AbvId = b.BeerId " +
                   "LEFT JOIN IBUs i ON i.IbuId = b.BeerId " +
                   "LEFT JOIN BeerStyles bs ON bs.BeerStyleId = b.BeerStyleId " +
                   "WHERE bb.BreweryId = @BreweryId", (breweryBeer, beer, srm, abv, ibu, beerStyle) =>
                   {
                       breweryBeer.Beer = beer;
                       if (srm != null)
                           beer.SRM = srm;
                       if (abv != null)
                           beer.ABV = abv;
                       if (ibu != null)
                           beer.IBU = ibu;
                       if (beerStyle != null)
                           beer.BeerStyle = beerStyle;
                       return breweryBeer;
                   }, new { brewery.BreweryId }, splitOn: "BeerId,SrmId,AbvId,IbuId,BeerStyleId");
                brewery.Beers = breweryBeers.ToList();

                var brewerySocials =
                    await context.QueryAsync<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                        new { brewery.BreweryId });
                brewery.Socials = brewerySocials.ToList();
                return brewery;
            }
        }

        public async Task AddAsync(Brewery brewery)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        brewery.UpdatedDate = DateTime.Now;
                        brewery.CreatedDate = DateTime.Now;
                        var breweryId =
                            await context.QueryAsync<int>(
                                "INSERT Breweries(Name,Description,Type,CreatedDate,UpdatedDate,Longitude,Latitude,Website,Established,HeaderImage,Avatar,OriginId,Address) " +
                                "VALUES (@Name,@Description,@Type,@CreatedDate,@UpdatedDate,@Longitude,@Latitude,@Website,@Established,@HeaderImage,@Avatar,@OriginId,@Address); " +
                                "SELECT CAST(SCOPE_IDENTITY() as int);", brewery, transaction);
                        brewery.BreweryId = breweryId.SingleOrDefault();
                        if (brewery.Socials != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT BrewerySocials(BreweryId,Site,Url) VALUES(@BreweryId,@Site,@Url);",
                                brewery.Socials.Select(u => new { brewery.BreweryId, u.Site, u.Url }), transaction);
                        }
                        if (brewery.Members != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT BreweryMembers(BreweryId,MemberUsername,Role) VALUES(@BreweryId,@MemberUsername,@Role);",
                                brewery.Members.Select(u => new { brewery.BreweryId, u.MemberUsername, u.Role }), transaction);
                        }
                        if (brewery.Beers != null)
                        {
                            await context.ExecuteAsync(
                                "INSERT BreweryBeers(BeerId,BreweryId) VALUES(@BeerId,@BreweryId);",
                                brewery.Beers.Select(b => new { b.BeerId, brewery.BreweryId }), transaction);
                        }
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Commit();
                        throw;
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Brewery brewery)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var result = await context.ExecuteAsync(
                            "UPDATE Breweries set Name = @Name,Description = @Name, Type = @Type, CreatedDate = @CreatedDate, UpdatedDate = @UpdatedDate," +
                            "Longitude = @Longitude, Latitude = @Latitude, Website = @Website, Established = @Established, HeaderImage = @HeaderImage, Avatar = @Avatar," +
                            "OriginId = @OriginId, Address = @Address WHERE BreweryId = @BreweryId;", brewery,
                            transaction);
                        await UpdateBrewerySocialsAsync(context, transaction, brewery);
                        await UpdateBreweryMembersAsync(context, transaction, brewery);
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

        public Task RemoveAsync(Brewery brewery)
        {
            throw new NotImplementedException();
        }

        public async Task<BreweryMember> GetSingleMemberAsync(int breweryId, string username)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var members =
                    await context.QueryAsync<BreweryMember>(
                        "SELECT * FROM BreweryMembers WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                        new { BreweryId = breweryId, MemberUsername = username });
                return members.SingleOrDefault();
            }
        }

        public async Task<IList<BreweryMember>> GetAllMembersAsync(int breweryId)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var members =
                    await context.QueryAsync<BreweryMember>(
                        "SELECT * FROM BreweryMembers WHERE BreweryId = @BreweryId;",
                        new { BreweryId = breweryId });
                return members.ToList();
            }
        }

        public async Task DeleteMember(int breweryId, string username)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync("DELETE FROM BreweryMembers WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                            new { BreweryId = breweryId, MemberUsername = username }, transaction);
                }
            }
        }

        public async Task UpdateMemberAsync(BreweryMember breweryMember)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync(
                        "UPDATE BreweryMembers set Role = @Role, Confirmed = @Confirmed WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                        breweryMember, transaction);
                }
            }
        }

        public async Task AddMemberAsync(BreweryMember breweryMember)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                using (var transaction = context.BeginTransaction())
                {
                    await context.ExecuteAsync("INSERT BreweryMembers(BreweryId,MemberUsername,Role,Confirmed) VALUES(@BreweryId,@MemberUsername,@Role,@Confirmed);",
                breweryMember, transaction);
                }
            }
        }

        public IList<BreweryMember> GetMemberships(string username)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var memberships =
                    context.Query<BreweryMember>("SELECT * FROM BreweryMembers WHERE MemberUsername = @MemberUsername;",
                        new {MemberUsername = username});
                return memberships.ToList();
            }
        }

        public IList<BreweryMember> GetMembers(int breweryId)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var members =
                     context.Query<BreweryMember>(
                        "SELECT * FROM BreweryMembers WHERE BreweryId = @BreweryId;",
                        new { BreweryId = breweryId });
                return members.ToList();
            }
        }

        public IList<BrewerySocial> GetBrewerySocials(int breweryId)
        {
            using (var context = DapperHelper.GetOpenConnection())
            {
                var socials =
                     context.Query<BrewerySocial>(
                        "SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId;",
                        new { BreweryId = breweryId });
                return socials.ToList();
            }
        }

        private void UpdateBrewerySocials(DbConnection context, DbTransaction transaction, Brewery brewery)
        {
            var brewerySocials = context.Query<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                new { brewery.BreweryId }, transaction);

            context.Execute("DELETE FROM BrewerySocials WHERE BreweryId = @BreweryId and SocialId = @SocialId;",
                brewerySocials.Where(
                    u => brewery.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            context.Execute(
                "UPDATE BrewerySocials set Site = @Site, Url = @Url WHERE BreweryId = @BreweryId and SocialId = @SocialId;",
                brewery.Socials, transaction);

            context.Execute("INSERT BrewerySocials(BreweryId,Site,Url) VALUES(@BreweryId,@Site,@Url);",
                brewery.Socials.Where(
                    s => brewerySocials.All(u => u.SocialId != s.SocialId)).Select(s => new { brewery.BreweryId, s.Site, s.Url }),
                transaction);
        }

        private async Task UpdateBrewerySocialsAsync(DbConnection context, DbTransaction transaction, Brewery brewery)
        {
            var brewerySocials = await context.QueryAsync<BrewerySocial>("SELECT * FROM BrewerySocials WHERE BreweryId = @BreweryId",
                new { brewery.BreweryId }, transaction);

            await context.ExecuteAsync("DELETE FROM BrewerySocials WHERE BreweryId = @BreweryId and SocialId = @SocialId;",
                brewerySocials.Where(
                    u => brewery.Socials.All(s => u.SocialId != s.SocialId)),
                transaction);

            await context.ExecuteAsync(
                "UPDATE BrewerySocials set Site = @Site, Url = @Url WHERE BreweryId = @BreweryId and SocialId = @SocialId;",
                brewery.Socials, transaction);

            await context.ExecuteAsync("INSERT BrewerySocials(BreweryId,Site,Url) VALUES(@BreweryId,@Site,@Url);",
                brewery.Socials.Where(
                    s => brewerySocials.All(u => u.SocialId != s.SocialId)).Select(s => new { brewery.BreweryId, s.Site, s.Url }),
                transaction);
        }

        private void UpdateBreweryMembers(DbConnection context, DbTransaction transaction, Brewery brewery)
        {
            var breweryMembers = context.Query<BreweryMember>(
                "SELECT * FROM BreweryMembers WHERE BreweryId = @BreweryId", new { brewery.BreweryId }, transaction);

            context.Execute("DELETE FROM BreweryMembers WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                breweryMembers.Where(bm => brewery.Members.All(m => bm.MemberUsername != m.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername }), transaction);

            context.Execute(
                "UPDATE BreweryMembers set Role = @Role, Confirmed = @Confirmed WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                brewery.Members.Where(m => breweryMembers.Any(bm => m.MemberUsername == bm.MemberUsername)), transaction);

            context.Execute("INSERT BreweryMembers(BreweryId,MemberUsername,Role,Confirmed) VALUES(@BreweryId,@MemberUsername,@Role,@Confirmed);",
                brewery.Members.Where(m => breweryMembers.All(bm => m.MemberUsername != bm.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername, bm.Role, bm.Confirmed }), transaction);

        }

        private async Task UpdateBreweryMembersAsync(DbConnection context, DbTransaction transaction, Brewery brewery)
        {
            var breweryMembers = await context.QueryAsync<BreweryMember>(
                "SELECT * FROM BreweryMembers WHERE BreweryId = @BreweryId", new { brewery.BreweryId }, transaction);

            await context.ExecuteAsync("DELETE FROM BreweryMembers WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                breweryMembers.Where(bm => brewery.Members.All(m => bm.MemberUsername != m.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername }), transaction);

            await context.ExecuteAsync(
                "UPDATE BreweryMembers set Role = @Role, Confirmed = @Confirmed WHERE BreweryId = @BreweryId and MemberUsername = @MemberUsername;",
                brewery.Members.Where(m => breweryMembers.Any(bm => m.MemberUsername == bm.MemberUsername)), transaction);

            await context.ExecuteAsync("INSERT BreweryMembers(BreweryId,MemberUsername,Role,Confirmed) VALUES(@BreweryId,@MemberUsername,@Role,@Confirmed);",
                brewery.Members.Where(m => breweryMembers.All(bm => m.MemberUsername != bm.MemberUsername)).Select(bm => new { brewery.BreweryId, bm.MemberUsername,bm.Role,bm.Confirmed }), transaction);

        }
    }
}
