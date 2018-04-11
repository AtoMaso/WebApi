using System.Linq;
using System.Web.Http.Filters;
using System;

namespace WebApi.Providers
{
    [AttributeUsage(AttributeTargets.Class | AttributeTargets.Method)]
    public class CompressFilter : ActionFilterAttribute
    {
        public override void OnActionExecuted(HttpActionExecutedContext context)
        {
            var acceptedEncoding = context.Response.RequestMessage.Headers.AcceptEncoding.First().Value;
            if (!acceptedEncoding.Equals("gzip", StringComparison.InvariantCultureIgnoreCase)
            && !acceptedEncoding.Equals("deflate", StringComparison.InvariantCultureIgnoreCase))
            {
                return;
            }
            context.Response.Content = new CompressedContent(context.Response.Content, acceptedEncoding);
        }
    }
}