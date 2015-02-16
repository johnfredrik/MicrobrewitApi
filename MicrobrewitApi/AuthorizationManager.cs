using System.Linq;
using System.Security.Claims;
using Microbrewit.Repository;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;

namespace Microbrewit.Api
{
    /// <summary>
    /// Handles the authorizations given for the different controllers.
    /// </summary>
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        private readonly IBreweryRepository _breweryRepository = new BreweryRepository();
        private readonly IBreweryElasticsearch _breweryElasticsearch = new BreweryElasticsearch();
        private readonly IBeerRepository _beerRepository= new BeerRepository();
        private readonly IBeerElasticsearch _beerElasticsearch = new BeerElasticsearch();

        public override bool CheckAccess(AuthorizationContext context)
        {

            var beerService = new BeerService(_beerElasticsearch,_beerRepository);

            if (!context.Principal.Identity.IsAuthenticated) return false;
            
            var username = context.Principal.Identity.Name;
            var memberships = _breweryRepository.GetMemberships(username);

            //User auth
            if ((context.Action.Any((c => c.Value == "Put")) || context.Action.Any(c => c.Value == "Upload") || context.Action.Any(c => c.Value == "Resend")) 
                && context.Resource.Any(c => c.Value == "User") && context.Principal.HasClaim(ClaimTypes.Role, "User") && username == context.Resource[1].Value)
                return true;

            // new Beer post auth
            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Resource.Any(c => c.Value.Equals("Beer")) && context.Principal.HasClaim(ClaimTypes.Role, "User"))
                return true;
            // New brewery post auth
            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Resource.Any(c => c.Value.Equals("Brewery")) && context.Principal.HasClaim(ClaimTypes.Role, "User"))
                return true;

            // Update beer auth: Brewer of beer or brewery memeber with role admin are allowed to change beer.
            if ((context.Action.Any(c => c.Value.Equals("Put")) || context.Action.Any(c => c.Value.Equals("Delete"))) && context.Resource.Any(c => c.Value.Equals("BeerId")))
            {
                int beerId;
                var success = int.TryParse(context.Resource[1].Value, out beerId);

                if (success && beerService.GetAllUserBeer(username).Any(b => b.Id.Equals(beerId)))
                    return true;

                if (memberships.Where(m => m.Role.Equals("Admin"))
                    .Any(b => beerService.GetAllBreweryBeers(b.BreweryId).Any(beer => beer.Id == beerId)))
                    return true;
            }

            if ((context.Action.Any(c => c.Value == "Delete" || c.Value == "Post" || c.Value == "Put" || c.Value == "Upload")) && 
                context.Resource.Any(c => c.Value.Equals("BreweryId")))
            {
                int breweryId;
                var success = int.TryParse(context.Resource.Last().Value, out breweryId);
                if (success && memberships.Any(m => m.Role != null && m.Role.Equals("Admin") && m.BreweryId == breweryId))
                    return true;

                if (success)
                {
                    var members = _breweryRepository.GetMembers(breweryId);
                    if (!members.Any() && context.Principal.HasClaim(ClaimTypes.Role, "Admin"))
                        return true;
                }
                
            }

            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Principal.HasClaim(ClaimTypes.Role, "Admin") &&
                context.Resource.Any(r => r.Value.Equals("Hop") || r.Value.Equals("Yeast") || r.Value.Equals("Fermentable")
                    || r.Value.Equals("Other") || r.Value.Equals("Supplier") || r.Value.Equals("Origin") || r.Value.Equals("BeerStyle")
                    || r.Value.Equals("Glass")))
                return true;
            
            if (context.Action.Any(c => c.Value.Equals("Delete")) && context.Principal.HasClaim(ClaimTypes.Role, "Admin") &&
                context.Resource.Any(r => r.Value.Equals("Hop") || r.Value.Equals("Yeast") || r.Value.Equals("Fermentable")
                    || r.Value.Equals("Other") || r.Value.Equals("Supplier") || r.Value.Equals("Origin") || r.Value.Equals("BeerStyle")
                    || r.Value.Equals("Glass")))
                return true;

            if (context.Action.Any(c => c.Value.Equals("Put")) && context.Principal.HasClaim(ClaimTypes.Role, "Admin") &&
                context.Resource.Any(r => r.Value.Equals("Hop") || r.Value.Equals("Yeast") || r.Value.Equals("Fermentable")
                    || r.Value.Equals("Other") || r.Value.Equals("Supplier") || r.Value.Equals("Origin") || r.Value.Equals("BeerStyle")
                    || r.Value.Equals("Glass")))
                return true;

            if (context.Action.Any(c => c.Value.Equals("Reindex")) && context.Principal.HasClaim(ClaimTypes.Role, "Admin") &&
                context.Resource.Any(r => r.Value.Equals("Hop") || r.Value.Equals("Yeast") || r.Value.Equals("Fermentable")
                    || r.Value.Equals("Other") || r.Value.Equals("Supplier") || r.Value.Equals("Origin") || r.Value.Equals("BeerStyle")
                    || r.Value.Equals("Glass") || r.Value.Equals("User") || r.Value.Equals("Beer")))
                return true;

            return false;
        }
    }
}