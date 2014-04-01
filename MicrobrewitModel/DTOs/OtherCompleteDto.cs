using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace Microbrewit.Model.DTOs
{
    public class OtherCompleteDto
    {
        public LinksOther Links { get; set; }
        public IList<OtherDto> Others { get; set; }
    }
}