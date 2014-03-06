using MicrobrewitApi.Controllers;
using MicrobrewitModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Routing;
using System.Web.Http;
using MicrobrewitApi.Util;

namespace MicrobrewitApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Database.SetInitializer(new InitializeDatabaseWithSeedData());
            // Web API configuration and services

            //config.Filters.Add(new TokenValidationAttribute());
            //config.Filters.Add(new CustomHttpsAttribute());         
            //config.Filters.Add(new BasicAuthenticationAttibute());
           // config.EnableCors();


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
           
        }
    }
}
