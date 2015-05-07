using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.Common;
using System.Data.Entity.ModelConfiguration.Conventions;
using System.Data.SqlClient;
using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Dapper;
using log4net;
using Microbrewit.Model;

namespace Microbrewit.Repository
{
    public class HopDapperRepository : IHopRepository
    {
        private static readonly string SqlConnection = ConfigurationManager.ConnectionStrings["MicrobrewitContext"].ConnectionString;
        private static readonly ILog Log = LogManager.GetLogger(MethodBase.GetCurrentMethod().DeclaringType);

        public IList<Hop> GetAll(params string[] navigationProperties)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                var sql = @"SELECT * FROM Hops h LEFT JOIN Origins o ON h.OriginId = o.OriginId";
                var hops = context.Query<Hop, Origin, Hop>(sql, (hop, origin) =>
                {
                    hop.Origin = origin;
                    hop.Flavours = new List<HopFlavour>();
                    hop.Substituts = new List<Hop>();
                    return hop;
                }, splitOn: "OriginId");

                var hopFlavours = context.Query<HopFlavour>("SELECT * FROM HopFlavours WHERE HopId in @Ids",
                    new { Ids = hops.Select(h => h.HopId).Distinct() });

                var flavours = context.Query<Flavour>("SELECT * FROM Flavours WHERE FlavourId in @Ids",
                    new { Ids = hopFlavours.Select(m => m.FlavourId).Distinct() });

                var substitutes = context.Query<Substitute>("SELECT * FROM Substitute WHERE HopId in @Ids",
                    new { Ids = hops.Select(h => h.HopId).Distinct() });

                foreach (var substitute in substitutes)
                {
                    var hop = hops.SingleOrDefault(h => h.HopId == substitute.HopId);
                    var sub = hops.SingleOrDefault(h => h.HopId == substitute.SubstituteId);
                    if (hop == null || sub == null) break;
                    if (hop.Substituts == null)
                        hop.Substituts = new List<Hop>();
                    hop.Substituts.Add(sub);
                }

                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                        hopFlavour.Flavour = flavour;
                    var hop = hops.SingleOrDefault(h => h.HopId == hopFlavour.HopId);
                    if (hop == null) break;
                    if (hop.Flavours == null)
                        hop.Flavours = new List<HopFlavour>();
                    hop.Flavours.Add(hopFlavour);
                }

                return hops.ToList();
            }

        }

        public Hop GetSingle(int id, params string[] navigationProperties)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                var sql = @"SELECT * FROM Hops h LEFT JOIN Origins o ON h.HopId = o.OriginId WHERE h.HopId = @Id";
                var hop = context.Query<Hop, Origin, Hop>(sql, (h, origin) =>
                {
                    h.Origin = origin;
                    h.Flavours = new List<HopFlavour>();
                    h.Substituts = new List<Hop>();
                    return h;
                }, new { Id = id }, splitOn: "HopId,OriginId"

                ).SingleOrDefault();

                if (hop == null) return null;

                var hopFlavours = context.Query<HopFlavour>("SELECT * FROM HopFlavours WHERE HopId = @id",
                  new { id = hop.HopId });

                var flavours = context.Query<Flavour>("SELECT * FROM Flavours WHERE FlavourId in @Ids",
                    new { Ids = hopFlavours.Select(m => m.FlavourId).Distinct() });

                var mapping = context.Query<Substitute>("SELECT * FROM Substitute WHERE HopId = @id",
                    new { id = hop.HopId });

                var substitutes = context.Query<Hop>("SELECT * FROM Hops WHERE HopId in @Ids",
                    new { Ids = mapping.Select(m => m.SubstituteId).Distinct() });

                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                        hopFlavour.Flavour = flavour;
                }

                hop.Flavours = hopFlavours.ToList();
                hop.Substituts = substitutes.ToList();

                return hop;
            }
        }

        public void Add(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql =
                            @"INSERT Hops(Name,AALow,AAHigh,BetaLow,BetaHigh,Notes,FlavourDescription,Custom,OriginId) 
                            VALUES(@name,@aaLow,@aaHigh,@betaLow,@betaHigh,@notes,@flavourDescription,@custom,@originId);
                            SELECT CAST(SCOPE_IDENTITY() as int)";

                        var id = context.Query<int>(sql,
                             new
                             {
                                 hop.Name,
                                 hop.AALow,
                                 hop.AAHigh,
                                 hop.BetaLow,
                                 hop.BetaHigh,
                                 hop.Notes,
                                 hop.FlavourDescription,
                                 hop.Custom,
                                 hop.OriginId
                             }, transaction).Single();

                        if (hop.Flavours != null)
                        {
                            context.Execute(
                                @"INSERT HopFlavours(FlavourId, HopId) VALUES(@FlavourId,@HopId);",
                                hop.Flavours.Select(h => new {h.FlavourId, HopId = id}),
                                transaction);
                        }

                        if (hop.Substituts != null)
                        {
                            context.Execute(
                                @"INSERT Substitute(HopId,SubstituteId) VALUES(@HopId,@SubstituteId);",
                                hop.Substituts.Select(s => new {HopId = id, SubstituteId = s.HopId}),
                                transaction);
                        }
                        transaction.Commit();
                        hop.HopId = id;
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Log.Error(e.ToString());
                        throw;
                    }
                }
            }
        }

        public void Update(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql =
                            @"Update Hops set Name = @name,AALow = @aaLow,AAHigh = @aaHigh, BetaLow = @betaLow,BetaHigh = @betaHigh, 
                            Notes = @notes,FlavourDescription = @flavourDescription, Custom = @custom,OriginId = @originId WHERE HopId = @hopId;";
                        context.Execute(sql,
                          new
                          {
                              hop.Name,
                              hop.AALow,
                              hop.AAHigh,
                              hop.BetaLow,
                              hop.BetaHigh,
                              hop.Notes,
                              hop.FlavourDescription,
                              hop.Custom,
                              hop.OriginId,
                              hop.HopId,
                          }, transaction);
                        UpdateHopFlavour(context, transaction, hop);
                        UpdateHopSubstitute(context, transaction, hop);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        throw;
                    }
                }
            }
        }

        private void UpdateHopSubstitute(DbConnection context, SqlTransaction transaction, Hop hop)
        {
            var hopSubstitutes = context.Query<Substitute>(@"SELECT * FROM Substitute WHERE HopId = @HopId",
                new {hop.HopId}, transaction);

            var toDelete = hopSubstitutes.Where(h => hop.Substituts.All(s => s.HopId != h.HopId && h.SubstituteId != s.HopId));
            context.Execute("DELETE FROM Substitute WHERE HopId = @HopId", toDelete, transaction);

            var toAdd = hop.Substituts.Where(h => hopSubstitutes.All(s => s.HopId != h.HopId && h.HopId != s.SubstituteId)).Select(c => new Substitute{HopId = hop.HopId, SubstituteId = c.HopId});
            context.Execute(@"INSERT Substitute(SubstituteId, HopId) VALUES(@SubstituteId,@HopId);", toAdd, transaction);
        }

        private void UpdateHopFlavour(DbConnection context, SqlTransaction transaction, Hop hop)
        {
            var hopFlavours = context.Query<HopFlavour>(@"SELECT * FROM HopFlavours WHERE HopId = @HopId", new {hop.HopId},
                transaction);

            var toDelete = hopFlavours.Where(h => hop.Flavours.All(f => f.FlavourId != h.FlavourId));
            context.Execute("DELETE FROM HopFlavours WHERE HopId = @HopId",
                toDelete.Select(h => new {h.HopId, h.FlavourId}), transaction);
                
            var toAdd = hop.Flavours.Where(h => hopFlavours.All(f => f.FlavourId != h.FlavourId));
            context.Execute(@"INSERT HopFlavours(FlavourId, HopId) VALUES(@FlavourId,@HopId);", toAdd, transaction);

        }

        public void Remove(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql = @"DELETE FROM Hops WHERE HopId = @HopId";
                        context.Execute(sql, new { HopId = hop.HopId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                    }
                }
            }
        }

        public async Task<IList<Hop>> GetAllAsync(params string[] navigationProperties)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                var sql = @"SELECT * FROM Hops h LEFT JOIN Origins o ON h.HopId = o.OriginId";
                var hops = await context.QueryAsync<Hop, Origin, Hop>(sql, (hop, origin) =>
                {
                    hop.Origin = origin;
                    return hop;
                }, splitOn: "HopId,OriginId");

                var hopFlavours = await context.QueryAsync<HopFlavour>("SELECT * FROM HopFlavours WHERE HopId in @Ids",
                    new { Ids = hops.Select(h => h.HopId).Distinct() });

                var flavours = await context.QueryAsync<Flavour>("SELECT * FROM Flavours WHERE FlavourId in @Ids",
                    new { Ids = hopFlavours.Select(m => m.FlavourId).Distinct() });

                var substitutes = await context.QueryAsync<Substitute>("SELECT * FROM Substitute WHERE HopId in @Ids",
                  new { Ids = hops.Select(h => h.HopId).Distinct() });

                foreach (var substitute in substitutes)
                {
                    var hop = hops.SingleOrDefault(h => h.HopId == substitute.HopId);
                    var sub = hops.SingleOrDefault(h => h.HopId == substitute.SubstituteId);
                    if (hop == null || sub == null) break;
                    if (hop.Substituts == null)
                        hop.Substituts = new List<Hop>();
                    hop.Substituts.Add(sub);
                }

                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                        hopFlavour.Flavour = flavour;
                    var hop = hops.SingleOrDefault(h => h.HopId == hopFlavour.HopId);
                    if (hop == null) break;
                    if (hop.Flavours == null)
                        hop.Flavours = new List<HopFlavour>();
                    hop.Flavours.Add(hopFlavour);
                }

                return hops.ToList();
            }
        }

        public async Task<Hop> GetSingleAsync(int id, params string[] navigtionProperties)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                var sql = @"SELECT * FROM Hops h LEFT JOIN Origins o ON h.HopId = o.OriginId WHERE h.HopId = @Id";
                var result = await context.QueryAsync<Hop, Origin, Hop>(sql, (h, origin) =>
                {
                    h.Origin = origin;
                    h.Flavours = new List<HopFlavour>();
                    h.Substituts = new List<Hop>();
                    return h;
                }, new { Id = id }, splitOn: "HopId,OriginId");

                var hop = result.SingleOrDefault();
                if (hop == null) return null;

                var hopFlavours = await context.QueryAsync<HopFlavour>("SELECT * FROM HopFlavours WHERE HopId = @id",
                  new { id = hop.HopId });

                var flavours = await context.QueryAsync<Flavour>("SELECT * FROM Flavours WHERE FlavourId in @Ids",
                    new { Ids = hopFlavours.Select(m => m.FlavourId).Distinct() });

                var mapping = await context.QueryAsync<Substitute>("SELECT * FROM Substitute WHERE HopId = @id",
                    new { id = hop.HopId });

                var substitutes = await context.QueryAsync<Hop>("SELECT * FROM Hops WHERE HopId in @Ids",
                    new { Ids = mapping.Select(m => m.SubstituteId).Distinct() });

                foreach (var hopFlavour in hopFlavours)
                {
                    var flavour = flavours.SingleOrDefault(f => f.FlavourId == hopFlavour.FlavourId);
                    if (flavour != null)
                        hopFlavour.Flavour = flavour;
                }

                hop.Flavours = hopFlavours.ToList();
                hop.Substituts = substitutes.ToList();

                return hop;
            }
        }

        public async Task AddAsync(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql =
                            @"INSERT Hops(Name,AALow,AAHigh,BetaLow,BetaHigh,Notes,FlavourDescription,Custom,OriginId) 
                            VALUES(@name,@aaLow,@aaHigh,@betaLow,@betaHigh,@notes,@flavourDescription,@custom,@originId);
                            SELECT CAST(SCOPE_IDENTITY() as int)";

                        var id = await context.QueryAsync<int>(sql,
                             new
                             {
                                 hop.Name,
                                 hop.AALow,
                                 hop.AAHigh,
                                 hop.BetaLow,
                                 hop.BetaHigh,
                                 hop.Notes,
                                 hop.FlavourDescription,
                                 hop.Custom,
                                 hop.OriginId
                             }, transaction);
                        transaction.Commit();
                        hop.HopId = id.Single();
                    }
                    catch (Exception e)
                    {
                        transaction.Rollback();
                        Log.Error(e.ToString());
                        throw;
                    }
                }
            }
        }

        public async Task<int> UpdateAsync(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql =
                            @"Update Hops set Name = @name,AALow = @aaLow,AAHigh = @aaHigh, BetaLow = @betaLow,BetaHigh = @betaHigh, 
                            Notes = @notes,FlavourDescription = @flavourDescription, Custom = @custom,OriginId = @originId WHERE HopId = @hopId;";
                        var result = await context.ExecuteAsync(sql,
                          new
                          {
                              hop.Name,
                              hop.AALow,
                              hop.AAHigh,
                              hop.BetaLow,
                              hop.BetaHigh,
                              hop.Notes,
                              hop.FlavourDescription,
                              hop.Custom,
                              hop.OriginId,
                              hop.HopId,
                          }, transaction);
                        transaction.Commit();
                        return result;
                    }
                    catch (Exception e)
                    {
                        Log.Error(e);
                        throw;
                    }
                }
            }
        }

        public async Task RemoveAsync(Hop hop)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    try
                    {
                        var sql = @"DELETE FROM Hops WHERE HopId = @HopId";
                        await context.ExecuteAsync(sql, new { hop.HopId }, transaction);
                        transaction.Commit();
                    }
                    catch (Exception e)
                    {
                        Log.Error(e.ToString());
                        transaction.Rollback();
                    }
                }
            }
        }

        public Flavour AddFlavour(string name)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                context.Open();
                using (var transaction = context.BeginTransaction())
                {
                    var flavourId = context.Query<int>("SELECT MAX(FlavourId) FROM Flavours",transaction: transaction).SingleOrDefault();
                    var flavour = context.Query<Flavour>(@"INSERT Flavours(FlavourId,Name) VALUES(@FlavourId,@Name); SELECT * FROM Flavours WHERE FlavourId = @FlavourId", new { FlavourId = flavourId + 1, Name = name }, transaction);
                    transaction.Commit();
                    return flavour.FirstOrDefault();

                }
            }
        }

        public HopForm GetForm(int id)
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                return context.Query<HopForm>(@"SELECT * FROM HopForms WHERE Id = @Id", new {Id = id}).SingleOrDefault();
            }
        }

        public async Task<IList<HopForm>> GetHopFormsAsync()
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                var hopForms = await context.QueryAsync<HopForm>("SELECT * FROM HopForms");
                return hopForms.ToList();
            }
        }

        public IList<HopForm> GetHopForms()
        {
            using (var context = new SqlConnection(SqlConnection))
            {
                return context.Query<HopForm>("SELECT * FROM HopForms").ToList();
            }
        }
    }
}
