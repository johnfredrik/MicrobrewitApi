using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microbrewit.Model;

namespace Microbrewit.Test
{
    public static class TestUtil
    {
        public static void DeleteDataInDatabase()
        {
            using(var context = new MicrobrewitContext())
	        {
                var sql = System.IO.File.ReadAllText(@"DeleteScript.txt");
                context.Database.ExecuteSqlCommand(sql);
	        }
        }
    }
}
