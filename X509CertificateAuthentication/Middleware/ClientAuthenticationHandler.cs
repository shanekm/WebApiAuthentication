namespace X509Authentication.Middleware
{
    using System.Diagnostics;
    using System.IdentityModel.Tokens;
    using System.Security.Claims;
    using System.Security.Cryptography.X509Certificates;
    using System.Threading.Tasks;
    using Microsoft.Owin.Security;
    using Microsoft.Owin.Security.Infrastructure;

    public class ClientCertificateAuthenticationHandler : AuthenticationHandler<ClientCertificateAuthenticationOptions>
    {
        protected override Task<AuthenticationTicket> AuthenticateCoreAsync()
        {
            var cert = this.Context.Get<X509Certificate2>("ssl.ClientCertificate");
            
            if (cert == null)
            {
                return Task.FromResult<AuthenticationTicket>(null);
            }

            // Using validator 
            //try
            //{
            //    this.Options.Validator.Validate(cert);
            //}
            //catch (SecurityTokenValidationException)
            //{
            //    return Task.FromResult<AuthenticationTicket>(null);
            //}

            //var identity = Identity.CreateFromCertificate(
            //    cert, 
            //    this.Options.AuthenticationType, 
            //    this.Options.CreateExtendedClaimSet);



            X509Chain chain = new X509Chain();
            X509ChainPolicy chainPolicy = new X509ChainPolicy()
            {
                RevocationMode = X509RevocationMode.NoCheck,
            };
            chain.ChainPolicy = chainPolicy;
            if (!chain.Build(cert))
            {
                foreach (X509ChainElement chainElement in chain.ChainElements)
                {
                    foreach (X509ChainStatus chainStatus in chainElement.ChainElementStatus)
                    {
                        Debug.WriteLine(chainStatus.StatusInformation);
                    }
                }
            }

            var identity = new ClaimsIdentity();

            var ticket = new AuthenticationTicket(identity, new AuthenticationProperties());
            return Task.FromResult<AuthenticationTicket>(ticket);
        }
    }
}