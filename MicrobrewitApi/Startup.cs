using System;
using System.Data.Entity;
using System.Net;
using System.Net.Http.Formatting;
using System;
 using System.Web.Http;
using System.Web.Mvc;
using System.Web.Optimization;
using System.Web.Routing;
using System.Web.SessionState;
using Microbrewit.Api.Automapper;
 using Microbrewit.Api.Provider;
using Microbrewit.HelpPage;
using Microbrewit.Model;
using Microbrewit.Model.Migrations;
 using Microsoft.Owin;
 using Microsoft.Owin.Security.OAuth;
 using Owin;
using WebApiContrib.Formatting.Jsonp;
 
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
            AreaRegistration.RegisterAllAreas();
            HttpConfiguration config = new HttpConfiguration();
 
            HttpConfiguration.AddJsonpFormatter();

            GlobalConfiguration.Configure(WebApiConfig.Register);
            HttpConfiguration.EnableCors();

            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfiguration.Configure();
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<MicrobrewitContext, Configuration>());
            //Database.SetInitializer(new MigrateDatabaseToLatestVersion<AuthContext, Model.AuthMigration.Configuration>());
             ConfigureOAuth(app);
 
            WebApiConfig.Register(HttpConfiguration);
            WebApiConfig.Register(config);
             app.UseCors(Microsoft.Owin.Cors.CorsOptions.AllowAll);
            app.UseWebApi(HttpConfiguration);
            app.UseWebApi(config);
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