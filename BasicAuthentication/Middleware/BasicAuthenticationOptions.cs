namespace BasicAuthentication.Middleware
{
    using Microsoft.Owin.Security;

    public class BasicAuthenticationOptions : AuthenticationOptions
    {
        public BasicAuthenticationOptions(
            string realm, 
            BasicAuthenticationMiddleware.CredentialVlidationFunction validationFunction)
            : base("Basic")
        {
            // Authentication type
            this.Realm = realm;
            this.CredentialValidationFunction = validationFunction;
        }

        public BasicAuthenticationMiddleware.CredentialVlidationFunction CredentialValidationFunction { get;
            private set; }

        public string Realm { get; private set; }
    }
}