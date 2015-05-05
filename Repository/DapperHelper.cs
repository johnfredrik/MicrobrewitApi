using System;
using System.Collections.Generic;
using System.Configuration;
using System.Data.SqlClient;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Repository
{
    public static class DapperHelper
    {
        private static readonly string SqlConnection =  ConfigurationManager.ConnectionStrings["MicrobrewitContext"].ConnectionString;

        public static SqlConnection GetConnection()
        {
            return new SqlConnection(SqlConnection);
        }
    }
}
