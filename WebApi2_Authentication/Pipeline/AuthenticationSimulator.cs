namespace WebApi2_Authentication.Pipeline
{
    using System;
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class AuthenticationSimulator
    {
        private readonly Func<IDictionary<string, object>, Task> _next;

        public AuthenticationSimulator(Func<IDictionary<string, object>, Task> next)
        {
            this._next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);
            context.Authentication.User = this.CreateUser();

            await this._next(env);
        }

        private ClaimsPrincipal CreateUser()
        {
            var claims = new List<Claim>
                             {
                                 new Claim(ClaimTypes.Role, "gee"), 
                                 new Claim(ClaimTypes.Surname, "foo_surname"), 
                                 new Claim(ClaimTypes.Name, "foo"), 
                                 new Claim(ClaimTypes.HomePhone, "foo_phone"), 
                                 new Claim(ClaimTypes.Role, "gee_2")
                             };

            var id = new ClaimsIdentity("Application", "claimTypeHere", "admin");
            id.AddClaims(claims);

            return new ClaimsPrincipal(id);
        }
    }
}