﻿using Microbrewit.Api.Controllers;
using Microbrewit.Model;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web.Routing;
using System.Web.Http;
using Microbrewit.Api.Util;

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

            config.Formatters.JsonFormatter.SerializerSettings.ReferenceLoopHandling = Newtonsoft.Json.ReferenceLoopHandling.Ignore;

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
