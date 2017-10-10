using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Owin;
using Owin;
using System.Web.Http;

[assembly: OwinStartup(typeof(WebApi.Startup))]

namespace WebApi
{
    public partial class Startup
    {
    public void Configuration(IAppBuilder app)
    {
      ConfigureAuth(app);
    }

    //// This method is required by Katana:
    //public void Configuration(IAppBuilder app)
    //{
    //  var webApiConfiguration = ConfigureWebApi();

    //  // Use the extension method provided by the WebApi.Owin library:
    //  app.UseWebApi(webApiConfiguration);
    //}


    //private HttpConfiguration ConfigureWebApi()
    //{
    //  var config = new HttpConfiguration();
    //  config.Routes.MapHttpRoute(
    //      "DefaultApi", "api/{controller}/{id}", new { id = RouteParameter.Optional });
    //  return config;
    //}

  }
}
