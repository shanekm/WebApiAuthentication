namespace BasicAuthentication.Middleware
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin;
    using Microsoft.Owin.Security.Infrastructure;

    public class BasicAuthenticationMiddleware : AuthenticationMiddleware<BasicAuthenticationOptions>
    {
        public delegate Task<IEnumerable<Claim>> CredentialVlidationFunction(string id, string section);

        public BasicAuthenticationMiddleware(OwinMiddleware next, BasicAuthenticationOptions options)
            : base(next, options)
        {
        }

        protected override AuthenticationHandler<BasicAuthenticationOptions> CreateHandler()
        {
            return new BasicAuthenticationHandler(this.Options);
        }
    }
}