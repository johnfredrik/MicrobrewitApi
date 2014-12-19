using System;
using System.Collections.Generic;
using System.Data.Entity.Migrations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Microbrewit.Model.Migrations
{
    public class Configuration : DbMigrationsConfiguration<MicrobrewitContext>
    {
        public Configuration()
        {
            AutomaticMigrationsEnabled = false;
            AutomaticMigrationDataLossAllowed = true;
        }

        protected override void Seed(MicrobrewitContext context)
        {
        }
    }
}
