using log4net;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;
using System.Web;

namespace Microbrewit.Api.MessageHandler
{
    public class MessageHandlerCors : DelegatingHandler
    {
        private static readonly ILog Log = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);
        protected override async Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
             Log.DebugFormat("Method: {0}", request.Method);

            // Call the inner handler.
            var response = await base.SendAsync(request, cancellationToken);
            if (request.Method == HttpMethod.Options)
            {

                if (request.Headers.GetValues("Origin") != null)
                {

                    var origin = request.Headers.GetValues("Origin").First();
                    response.Headers.Add("Access-Control-Allow-Origin", origin);
                }
            }
            //Log.DebugFormat("Authority: http://{0} Request URI: {1}", request.RequestUri.Authority, request.RequestUri);
            //response.Headers.Add("Access-Control-Allow-Origin", "http://" + request.RequestUri.Authority.ToString());
            return response;
        }
    }
}