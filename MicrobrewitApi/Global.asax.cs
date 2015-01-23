using System.Web;

namespace Microbrewit.Api
{
    public class WebApiApplication : HttpApplication
    {
        protected void Application_Start()
        {
            //AutoMapperConfiguration.Configure();
            //// Add this code, if not present.
            //AreaRegistration.RegisterAllAreas();
           
            ////GlobalConfiguration.Configuration.AddJsonpFormatter();
            
            //GlobalConfiguration.Configure(WebApiConfig.Register);
            ////GlobalConfiguration.Configuration.EnableCors();
            
            //FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            //RouteConfig.RegisterRoutes(RouteTable.Routes);
            //BundleConfig.RegisterBundles(BundleTable.Bundles);

            //AutoMapperConfiguration.Configure();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MicrobrewitContext,Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext,Model.AuthMigration.Configuration>());         
        }
    }
}
