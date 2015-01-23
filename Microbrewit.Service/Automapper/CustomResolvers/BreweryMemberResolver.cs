using System.Collections.Generic;
using AutoMapper;
using Microbrewit.Model;
using Microbrewit.Model.DTOs;

namespace Microbrewit.Service.Automapper.CustomResolvers
{
    public class BreweryMemberResolver : ValueResolver<BreweryDto,IList<BreweryMember>>
    {
        protected override IList<BreweryMember> ResolveCore(BreweryDto source)
        {
            
            var members = new List<BreweryMember>();
            if (source.Members == null) return members;

            foreach (var memberDto in source.Members)
            {
                var member = new BreweryMember()
                {
                    BreweryId = source.Id,
                    MemberUsername = memberDto.Username,
                    //Role = memberDto.Role,
                };
                members.Add(member);
            }
            return members;
        }
    }
}