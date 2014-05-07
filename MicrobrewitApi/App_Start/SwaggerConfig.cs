using System.Web.Http;
using Microbrewit.Api;
using WebActivatorEx;
using Swashbuckle.Application;

[assembly: PreApplicationStartMethod(typeof(SwaggerConfig), "Register")]

namespace Microbrewit.Api
{
    public class SwaggerConfig
    {
        public static void Register()
        {
            Swashbuckle.Bootstrapper.Init(GlobalConfiguration.Configuration);

            // NOTE: If you want to customize the generated swagger or UI, use SwaggerSpecConfig and/or SwaggerUiConfig here ...

            SwaggerUiConfig.Customize(c =>
            {
              
                c.InjectJavaScript(typeof(SwaggerConfig).Assembly, "Microbrewit.Api.Scripts.test.js");
                
            });
        }
    }
}