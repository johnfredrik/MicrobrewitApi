using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Security.Claims;
using System.Threading;
using System.Web;
using Microbrewit.Repository;
using Claim = System.IdentityModel.Claims.Claim;

namespace Microbrewit.Api
{
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        private IBeerRepository _beerRepository = new BeerRepository();
        private IBreweryRepository _breweryRepository = new BreweryRepository();

        public override bool CheckAccess(AuthorizationContext context)
        {
            if (!context.Principal.Identity.IsAuthenticated) return false;

            

            if (context.Action.Any(c => c.Value.Equals("Post")) && context.Resource.Any(c => c.Value.Equals("Beer")) && context.Principal.HasClaim(ClaimTypes.Role, "User"))
                return true;

            if (context.Action.Any(c => c.Value.Equals("Put")) && context.Resource.Any(c => c.Value.Equals("BeerId")))
            {
                int beerId;
                int.TryParse(context.Resource[1].Value, out beerId);
                var username = context.Principal.Identity.Name;

                if (_beerRepository.GetAllUserBeer(username).Any(b => b.Id.Equals(beerId)))
                    return true;


                if (_breweryRepository.GetBreweryMemberships(username).Where(m => m.Role.Equals("Admin"))
                    .Any(b => _beerRepository.GetAllBreweryBeers(b.BreweryId).Any(beer => beer.Id == beerId)))
                    return true;


            }


            if (context.Principal.HasClaim(ClaimTypes.Role, "Admin"))
                return true;


            return false;
        }
    }
}