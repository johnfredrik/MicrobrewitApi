using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Repository.Repository;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class UserEmailConfirmedResolver : ValueResolver<User,bool>
    {
        private AuthRepository _authRepository = new AuthRepository();

        protected override bool ResolveCore(User source)
        {
            if (source == null) return false;
            var user =  _authRepository.FindUserByName(source.Username);
            return user != null && _authRepository.IsEmailConfirmed(user.Id);
        }
    }
}