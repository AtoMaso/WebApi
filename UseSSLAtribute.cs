﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using System.Web.Http.Filters;

namespace iTradeWebApi
{
     public class UseSSLAttribute : AuthorizationFilterAttribute
     {
          public override void OnAuthorization(System.Web.Http.Controllers.HttpActionContext actionContext)
          {
                if (actionContext.Request.RequestUri.Scheme != Uri.UriSchemeHttps)
               {
                    HttpResponseMessage msg = new HttpResponseMessage();
                    msg.StatusCode = HttpStatusCode.Forbidden;
                    msg.ReasonPhrase = "SSL Needed!";
                    actionContext.Response = msg;
               }
               else
               {
                    base.OnAuthorization(actionContext);
                }
          }
     }

}
