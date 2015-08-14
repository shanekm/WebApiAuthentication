using System.Web.Http;

namespace WebApi2_Authentication
{
    using WebApi2_Authentication.Pipeline;

    public static class WebApiConfig
    {
        public static void Register(HttpConfiguration config)
        {
            // Web API configuration and services

            // Web API routes
            config.MapHttpAttributeRoutes();

            config.Routes.MapHttpRoute(
                name: "DefaultApi",
                routeTemplate: "api/{controller}/{id}",
                defaults: new { id = RouteParameter.Optional }
            );

            // remove xml formatter
            config.Formatters.Remove(config.Formatters.XmlFormatter);

            // Run filter on all controllers/actions
            //config.Filters.Add(new CustomAuthorizationAttribute());
        }
    }
}
