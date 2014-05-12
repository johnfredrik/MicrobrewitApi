using Swashbuckle.Swagger;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Web;
using System.Web.Http.Description;

namespace Microbrewit.Api.Swagger
{
        public class AddStandardResponseCodes : IOperationFilter
    {
        public void Apply(Operation operation, DataTypeRegistry dataTypeRegistry, ApiDescription apiDescription)
        {
            //operation.ResponseMessages.Add(new ResponseMessage
            //{
            //    Code = (int)HttpStatusCode.OK,
            //    Message = "It's all good!"
            //});

            operation.ResponseMessages.Add(new ResponseMessage
            {
                Code = (int)HttpStatusCode.InternalServerError,
                Message = "Somethings up!"
            });
        }
    }
}