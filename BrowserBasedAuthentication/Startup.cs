using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(BrowserBasedAuthentication.Startup))]
namespace BrowserBasedAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            ConfigureAuth(app);
        }
    }
}
