using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using log4net;
using Microbrewit.Model.Model;

namespace Microbrewit.Model
{
    public class InitializeAuthDatabase : DropCreateDatabaseAlways<AuthContext>
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        protected override void Seed(AuthContext context)
        {
            Log.Debug("Initilizing DataBase with Seed Data");

            

        }
    }
}
