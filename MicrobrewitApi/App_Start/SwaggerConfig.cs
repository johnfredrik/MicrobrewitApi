using System.Web.Http;
using Microbrewit.Api;
using WebActivatorEx;
using Swashbuckle.Application;
using System;
using Microbrewit.Api.Swagger;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Microbrewit.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            Swashbuckle.Bootstrapper.Init(GlobalConfiguration.Configuration);

            // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...
            SwaggerSpecConfig.Customize(c =>
            {
                //c.IgnoreObsoleteActions();
                c.OperationFilter<AddStandardResponseCodes>();
                c.IncludeXmlComments(GetXmlCommentsPath());
            });

            SwaggerUiConfig.Customize(c =>
            {
                c.SupportHeaderParams = true;
                c.DocExpansion = DocExpansion.Full;
                c.InjectJavaScript(typeof(SwaggerConfig).Assembly, "Microbrewit.Api.Scripts.test.js");
                
            });
        }

        private static string GetXmlCommentsPath()
        {
            return String.Format(@"{0}App_Data\XmlDocument.xml", AppDomain.CurrentDomain.BaseDirectory);
        }
    }
}