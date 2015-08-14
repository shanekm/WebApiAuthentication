namespace BasicAuthentication.Middleware
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;

    public class BasicAuthenticationHandler : AuthenticationHandler<BasicAuthenticationOptions>
    {
        private readonly string _challenge;

        public BasicAuthenticationHandler(BasicAuthenticationOptions options)
        {
            this._challenge = "Basic realm=" + options.Realm;
        }

        protected override async Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var authValue = this.Request.Headers.Get("Authorization");
            if (string.IsNullOrEmpty(authValue) || !authValue.StartsWith("Basic ", StringComparison.OrdinalIgnoreCase))
            {
                return null;
            }

            var token = authValue.Substring("Basic ".Length).Trim();
            var claims =
                await this.TryGetPrincipalFromBasicCredentials(token, Options.CredentialValidationFunction);

            if (claims == null)
            {
                return null;
            }
            else
            {
                var id = new ClaimsIdentity(claims, this.Options.AuthenticationType);
                return new AuthenticationTicket(id, new AuthenticationProperties());
            }
        }

        protected override Task ApplyResponseChallengeAsync()
        {
            if (this.Response.StatusCode == 401)
            {
                var challenge = this.Helper.LookupChallenge(
                    this.Options.AuthenticationType, 
                    this.Options.AuthenticationMode);
                if (challenge != null)
                {
                    this.Response.Headers.AppendValues("WWW-Authenticate", this._challenge);
                }
            }

            return Task.FromResult<object>(null);
        }

        private async Task<IEnumerable<Claim>> TryGetPrincipalFromBasicCredentials(
            string credentials,
            BasicAuthenticationMiddleware.CredentialVlidationFunction validate)
        {
            string pair;
            try
            {
                pair = Encoding.UTF8.GetString(Convert.FromBase64String(credentials));
            }
            catch (FormatException)
            {
                return null;
            }
            catch (ArgumentException)
            {
                return null;
            }

            var ix = pair.IndexOf(':');
            if (ix == -1)
            {
                return null;
            }

            var username = pair.Substring(0, ix);
            var pw = pair.Substring(ix + 1);

            return await validate(username, pw);
        }
    }
}