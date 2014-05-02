using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Web.Http;
using System.Web.Routing;
using AutoMapper;
using Microbrewit.Api.Automapper;
using System.Net.Http.Formatting;
using WebApiContrib.Formatting.Jsonp;
using System.Web.Mvc;
using Microbrewit.HelpPage;
using System.Web.Optimization;

namespace Microbrewit.Api
{
    public class WebApiApplication : System.Web.HttpApplication
    {
        protected void Application_Start()
        {
            // Add this code, if not present.
            AreaRegistration.RegisterAllAreas();
           
            GlobalConfiguration.Configuration.AddJsonpFormatter();
            
            GlobalConfiguration.Configure(WebApiConfig.Register);
           
            
            FilterConfig.RegisterGlobalFilters(GlobalFilters.Filters);
            RouteConfig.RegisterRoutes(RouteTable.Routes);
            BundleConfig.RegisterBundles(BundleTable.Bundles);

            AutoMapperConfiguration.Configure();
            Database.SetInitializer(new InitializeDatabaseWithSeedData());
           
        }
    }
}
