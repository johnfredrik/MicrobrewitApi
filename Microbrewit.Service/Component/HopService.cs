using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Microbrewit.Model.DTOs;
using Microbrewit.Service.Interface;

namespace Microbrewit.Service.Component
{
    public class HopService : IHopService
    {
        public Task<IList<HopDto>> GetHopsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<HopDto> GetHopAsync()
        {
            throw new NotImplementedException();
        }
    }
}
