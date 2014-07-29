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
using System.Web;
using Newtonsoft.Json;
using Newtonsoft.Json.Serialization;

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
          

            //config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

            //dependency injection
            var container = new UnityContainer();
            container.RegisterType<IBeerRepository, BeerRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerStyleRepository, BeerStyleRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBreweryRepository, BreweryRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFermentableRepository, FermentableRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IHopRepository, HopRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOriginRespository, OriginRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOtherRepository, OtherRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupplierRepository, SupplierRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IYeastRepository, YeastRepository>(new HierarchicalLifetimeManager());
           
            

            config.DependencyResolver = new UnityResolver(container);
            //// Web API routes
            config.MapHttpAttributeRoutes();

            //var cors = new EnableCorsAttribute("*",
            //                                     "*",
            //                                     "*");

            //config.EnableCors();
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );
            
            var formatters = GlobalConfiguration.Configuration.Formatters;
            var jsonFormatter = formatters.JsonFormatter;
            var settings = jsonFormatter.SerializerSettings;
            settings.Formatting = Formatting.Indented;
            settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);

          

        }
    }
}
