namespace Owin
{
    using BasicAuthentication.Middleware;

    public static class BasicAuthenticationMiddlewareExtensions
    {
        public static IAppBuilder UseBasicAuthentication(this IAppBuilder appBuilder, string realm, BasicAuthenticationMiddleware.CredentialVlidationFunction validationFunction)
        {
            var options = new BasicAuthenticationOptions(realm, validationFunction);
            return appBuilder.UseBasicAuthentication(options);
        }

        public static IAppBuilder UseBasicAuthentication(this IAppBuilder appBuilder, BasicAuthenticationOptions options)
        {
            return appBuilder.Use<BasicAuthenticationMiddleware>(options);
        }
    }
}