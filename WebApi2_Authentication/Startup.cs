using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(WebApi2_Authentication.Startup))]
namespace WebApi2_Authentication
{
    using System.Diagnostics;
    using System.Security.Claims;

    using WebApi2_Authentication.Pipeline;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.Use(typeof(TestOwinMiddleware));

            // Claims manager
           // ClaimsAuthorizationManager 

            // Create a user
            app.Use(typeof(AuthenticationSimulator));

            ConfigureAuth(app);
        }
    }
}
