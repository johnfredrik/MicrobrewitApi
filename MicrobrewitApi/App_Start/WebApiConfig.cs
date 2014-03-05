using MicrobrewitModel;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Http;

namespace MicrobrewitApi
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            Database.SetInitializer(new InitializeDatabaseWithSeedData());
            // Web API configuration and services

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
