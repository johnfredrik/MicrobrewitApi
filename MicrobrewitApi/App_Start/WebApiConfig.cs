using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Routing;
using System.Web.Http;
using Microbrewit.Api.Util;
using WebApiContrib.Formatting.Jsonp;
using Microsoft.Practices.Unity;
using Microbrewit.Repository;

namespace Microbrewit.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
           
            // Web API configuration and services

            //config.Filters.Add(new TokenValidationAttribute());
            //config.Filters.Add(new CustomHttpsAttribute());         
            //config.Filters.Add(new BasicAuthenticationAttibute());
           // config.EnableCors();

            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //dependency injection
            var container = new UnityContainer();
            container.RegisterType<IOtherRepository, OtherRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerRepository, BeerRepository>(new HierarchicalLifetimeManager());
            config.DependencyResolver = new UnityResolver(container);


            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);
           
        }
    }
}
