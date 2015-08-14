namespace Owin
{
    using X509Authentication.Middleware;

    public static class ClientAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseClientCertificateAuthentication(this IAppBuilder appBuilder)
        {
            var options = new ClientCertificateAuthenticationOptions();
            return appBuilder.UseClientCertificateAuthentication(options);
        }

        public static IAppBuilder UseClientCertificateAuthentication(this IAppBuilder appBuilder, ClientCertificateAuthenticationOptions options)
        {
            return appBuilder.Use<ClientAuthenticationMiddleware>(options);
        }
    }
}