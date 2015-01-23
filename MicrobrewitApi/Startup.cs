using System;
using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using Microbrewit.Api.Provider;
using Microbrewit.HelpPage;
using Microsoft.Owin;
using Microsoft.Owin.Cors;
using Microbrewit.Service;
using Microbrewit.Service.Automapper;
using Microsoft.Owin.Security.OAuth;
using Owin;

namespace Microbrewit.Api
{
    public class Startup
    {
        public static HttpConfiguration HttpConfiguration { get; private set; }

        public Startup()
        {
            HttpConfiguration = new HttpConfiguration();
        }

        public void Configuration(IAppBuilder app)
        {
            //HttpConfiguration config = new HttpConfiguration();
            // Add this code, if not present.
            // Add this code, if not present.
            AreaRegistration.RegisterAllAreas();

            //GlobalConfiguration.Configuration.AddJsonpFormatter();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            //GlobalConfiguration.Configuration.EnableCors();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfiguration.Configure();

            // HttpConfiguration config = new HttpConfiguration();

            //HttpConfiguration.AddJsonpFormatter();

            HttpConfiguration.EnableCors();




            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MicrobrewitContext, Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Model.AuthMigration.Configuration>());
            ConfigureOAuth(app);
            //WebApiConfig.Register(HttpConfiguration);
            //WebApiConfig.Register(config);
            app.UseCors(CorsOptions.AllowAll);
            app.UseWebApi(HttpConfiguration);
            //app.UseWebApi(config);
        }

        public void ConfigureOAuth(IAppBuilder app)
        {
            OAuthAuthorizationServerOptions OAuthServerOptions = new OAuthAuthorizationServerOptions
            {
                AllowInsecureHttp = true,
                TokenEndpointPath = new PathString("/token"),
                AccessTokenExpireTimeSpan = TimeSpan.FromMinutes(30),
                Provider = new SimpleAuthorizationServerProvider(),
                RefreshTokenProvider = new SimpleRefreshTokenProvider()
            };

            app.UseOAuthAuthorizationServer(OAuthServerOptions);
            app.UseOAuthBearerAuthentication(new OAuthBearerAuthenticationOptions());

        }
    }
}