namespace X509Authentication.Middleware
{
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Infrastructure;


    public class ClientAuthenticationMiddleware : AuthenticationMiddleware<ClientCertificateAuthenticationOptions>
    {
        public ClientAuthenticationMiddleware(OwinMiddleware next, ClientCertificateAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<ClientCertificateAuthenticationOptions> CreateHandler()
        {
            return new ClientCertificateAuthenticationHandler();
        }
    }
}