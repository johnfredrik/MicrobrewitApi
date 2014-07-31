using log4net;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.Http.ExceptionHandling;

namespace Microbrewit.Api.ErrorHandler
{
    public class Log4NetExceptionLogger : ExceptionLogger
    {
        private static readonly ILog Logger = LogManager.GetLogger(System.Reflection.MethodBase.GetCurrentMethod().DeclaringType);

        public override void Log(ExceptionLoggerContext context)
        {
            Logger.Error(context.ExceptionContext.Exception.ToString());       
        }
    }
}