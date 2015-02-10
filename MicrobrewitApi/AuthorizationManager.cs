using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Microbrewit.Repository;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
using Microbrewit.Service.Interface;
using Claim = System.IdentityModel.Claims.Claim;

namespace Microbrewit.Api
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        private readonly IBreweryRepository _breweryRepository = new BreweryRepository();
        private readonly IBreweryElasticsearch _breweryElasticsearch = new BreweryElasticsearch();
        private readonly IBeerRepository _beerRepository= new BeerRepository();
        private readonly IBeerElasticsearch _beerElasticsearch = new BeerElasticsearch();

        //public AuthorizationManager(IBeerService beerService, IBreweryService breweryService)
        //{
        //    _beerService = beerService;
        //    _breweryService = breweryService;
        //    _breweryRepository = new BreweryRepository();
        //}

        public override bool CheckAccess(AuthorizationContext context)
        {
            var beerService = new BeerService(_beerElasticsearch,_beerRepository);

            if (!context.Principal.Identity.IsAuthenticated) return false;
            
            var username = context.Principal.Identity.Name;
            var memberships = _breweryRepository.GetMemberships(username);


            if ((context.Action.Any((c => c.Value == "Put")) || context.Action.Any(c => c.Value == "Upload") || context.Action.Any(c => c.Value == "Resend")) 
                && context.Resource.Any(c => c.Value == "Users") && context.Principal.HasClaim(ClaimTypes.Role, "User") && username == context.Resource[1].Value)
                return true;

            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Resource.Any(c => c.Value.Equals("Beer")) && context.Principal.HasClaim(ClaimTypes.Role, "User"))
                return true;

            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Resource.Any(c => c.Value.Equals("Brewery")) && context.Principal.HasClaim(ClaimTypes.Role, "User"))
                return true;

            if (context.Action.Any(c => c.Value.Equals("Put")) && context.Resource.Any(c => c.Value.Equals("BeerId")))
            {
                int beerId;
                var success = int.TryParse(context.Resource[1].Value, out beerId);

                if (success && beerService.GetAllUserBeer(username).Any(b => b.Id.Equals(beerId)))
                    return true;


                if (memberships.Where(m => m.Role.Equals("Admin"))
                    .Any(b => beerService.GetAllBreweryBeers(b.BreweryId).Any(beer => beer.Id == beerId)))
                    return true;
            }

            if (context.Action.Any(c => c.Value.Equals("Delete")) && context.Resource.Any(c => c.Value.Equals("BeerId")))
            {
                int beerId;
                var success = int.TryParse(context.Resource[1].Value, out beerId);
                if (success)
                {
                    var beer = beerService.GetSingle(beerId);
                    if (beer.Brewers.Any(b => b.Username.Equals(username)))
                        return true;
                    if (memberships.Any(m => m.Role.Equals("Admin") && beer.Breweries.Any(b => b.Id == m.BreweryId)))
                        return true;
                }
            }

            if ((context.Action.Any(c => c.Value == "Delete" || c.Value == "Post" || c.Value == "Put" || c.Value == "Upload")) && 
                context.Resource.Any(c => c.Value.Equals("BreweryId")))
            {
                int breweryId;
                var success = int.TryParse(context.Resource.Last().Value, out breweryId);
                if (success && memberships.Any(m => m.Role.Equals("Admin") && m.BreweryId == breweryId))
                {
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