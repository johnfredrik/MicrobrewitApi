using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;
using Microbrewit.Repository.Repository;

namespace Microbrewit.Api.Automapper.CustomResolvers
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