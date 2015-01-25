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
using System.Web.Http.ExceptionHandling;
using Microbrewit.Api.ErrorHandler;
using Microbrewit.Service.Component;
using Microbrewit.Service.Interface;

namespace Microbrewit.Api
{
    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {                     
            // Web API configuration and services
     
            //dependency injection
            var container = new UnityContainer();
            // Repository
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
            container.RegisterType<IUserCredentialRepository, UserCredentialRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGlassRepository, GlassRepository>(new HierarchicalLifetimeManager());
            //Services
            container.RegisterType<IHopService, HopService>(new HierarchicalLifetimeManager());
            

            config.DependencyResolver = new UnityResolver(container);
            //// Web API routes
            config.MapHttpAttributeRoutes();                 
            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // There can be multiple exception loggers. (By default, no exception loggers are registered.)
            config.Services.Add(typeof(IExceptionLogger), new Log4NetExceptionLogger());


            // There must be exactly one exception handler. (There is a default one that may be replaced.)
            // To make this sample easier to run in a browser, replace the default exception handler with one that sends
            // back text/plain content for all errors.
            config.Services.Replace(typeof(IExceptionHandler), new GlobalExceptionHandler());
            config.AddJsonpFormatter();
            //var formatters = GlobalConfiguration.Configuration.Formatters;
            //var jsonFormatter = formatters.JsonFormatter;
            //var settings = jsonFormatter.SerializerSettings;
            //settings.Formatting = Formatting.Indented;
            //settings.ContractResolver = new CamelCasePropertyNamesContractResolver();

            var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);          
        }
    }
}
