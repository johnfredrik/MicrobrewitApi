using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Web.Configuration;

namespace Microbrewit.Model
{
    public static class Setting
    {
        public static readonly string ElasticSearchIndex = WebConfigurationManager.AppSettings["elasticsearchIndex"];
    }
}
