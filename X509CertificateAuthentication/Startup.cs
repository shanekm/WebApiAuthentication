using Microsoft.Owin;
using Owin;

[assembly: OwinStartupAttribute(typeof(X509CertificateAuthentication.Startup))]
namespace X509CertificateAuthentication
{
    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            //ConfigureAuth(app);
            app.UseClientCertificateAuthentication();
        }
    }
}
