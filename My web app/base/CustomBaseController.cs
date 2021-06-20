using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using My_web_app.common;
using Newtonsoft.Json;
using static My_web_app.common.ResponseStatus;

namespace My_web_app.helper
{
    public class CustomBaseController : Controller
    {
        private HttpResponse httpResponse;
        private IConfiguration configuration;

        public CustomBaseController(IConfiguration iConfiguration)
        {
            this.configuration = iConfiguration;
        }


        [NonAction]
        public void SendResponse(ResponseHandler responseHandler)
        {
            string message_description = null;
            string message_content = null;
            HttpContext.Response.ContentType = "application/json";

            ResponseStatus? rStatus = responseHandler._responseStatus;

            if (responseHandler._Msg != null)
            {
                message_description = responseHandler._Msg;
                message_content = configuration["messages:" + responseHandler._Msg];
            }

            if (rStatus != null)
            {
                switch (rStatus)
                {
                    case OK:
                        HttpContext.Response.StatusCode = 200;
                        break;
                    case NOT_FOUND:
                        HttpContext.Response.StatusCode = 404;
                        break;
                    case NOT_ACCEPT:
                        HttpContext.Response.StatusCode = 406;
                        break;
                    case SERVER_ERROR:
                        HttpContext.Response.StatusCode = 500;
                        break;
                    default:
                        HttpContext.Response.StatusCode = 200;
                        break;
                }
            }

            if (rStatus == null && responseHandler._Msg == null)
            {
                switch (HttpContext.Request.Method.ToLower())
                {
                    case "get":
                        message_content = configuration["messages:get_success"];
                        break;
                    case "post":
                        message_content = configuration["messages:insert_success"];
                        break;
                    case "put":
                        message_content = configuration["messages:update_success"];
                        break;
                    case "delete":
                        message_content = configuration["messages:delete_success"];
                        break;
                }
            }


            HttpContext.Response.WriteAsync(JsonConvert.SerializeObject(new
            {
                data = responseHandler._Data,
                message_description,
                message_content,
            }));
        }
    }
}