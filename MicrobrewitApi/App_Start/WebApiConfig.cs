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
using Microbrewit.Repository.Repository;
using Microbrewit.Service.Component;
using Microbrewit.Service.Elasticsearch.Component;
using Microbrewit.Service.Elasticsearch.Interface;
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
            container.RegisterType<IBeerRepository, BeerDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerStyleRepository, BeerStyleDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IBreweryRepository, BreweryDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IFermentableRepository, FermentableDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IHopRepository, HopDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOriginRespository, OriginDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IOtherRepository, OtherDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupplierRepository, SupplierDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserRepository, UserDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IYeastRepository, YeastDapperRepository>(new HierarchicalLifetimeManager());
            container.RegisterType<IGlassRepository, GlassDapperRepository>(new HierarchicalLifetimeManager());
            //Services
            container.RegisterType<IHopService, HopService>(new HierarchicalLifetimeManager());
            container.RegisterType<IYeastService, YeastService>(new HierarchicalLifetimeManager());
            container.RegisterType<IFermentableService, FermentableService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOtherService, OtherService>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupplierService, SupplierService>(new HierarchicalLifetimeManager());
            container.RegisterType<IOriginService, OriginService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerStyleService, BeerStyleService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBreweryService, BreweryService>(new HierarchicalLifetimeManager());
            container.RegisterType<IGlassService, GlassService>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserService, UserService>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerService, BeerService>(new HierarchicalLifetimeManager());

            //Elasticsearch
            container.RegisterType<IHopElasticsearch, HopElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IYeastElasticsearch, YeastElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IFermentableElasticsearch, FermentableElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IOtherElasticsearch, OtherElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<ISupplierElasticsearch, SupplierElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<ISearchElasticsearch, SearchElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IOriginElasticsearch, OriginElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerStyleElasticsearch, BeerStyleElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IBreweryElasticsearch, BreweryElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IGlassElasticsearch, GlassElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IUserElasticsearch, UserElasticsearch>(new HierarchicalLifetimeManager());
            container.RegisterType<IBeerElasticsearch, BeerElasticsearch>(new HierarchicalLifetimeManager());

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

            //var appXmlType = config.Formatters.XmlFormatter.SupportedMediaTypes.FirstOrDefault(t => t.MediaType == "application/xml");
            //config.Formatters.XmlFormatter.SupportedMediaTypes.Remove(appXmlType);   
            config.Formatters.XmlFormatter.UseXmlSerializer = true;
        }
    }
}
