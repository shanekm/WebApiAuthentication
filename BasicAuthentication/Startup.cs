using Microsoft.Owin;

[assembly: OwinStartup(typeof(BasicAuthentication.Startup))]

namespace BasicAuthentication
{
    using System.Collections.Generic;
    using System.Security.Claims;
    using System.Threading.Tasks;
    using Owin;

    public partial class Startup
    {
        public void Configuration(IAppBuilder app)
        {
            app.UseBasicAuthentication("Demo", this.ValidateUsers);
        }

        private async Task<IEnumerable<Claim>> ValidateUsers(string id, string secret)
        {
            // do db lookup here
            if (id == secret)
            {
                var claims = new List<Claim>
                                 {
                                     new Claim(ClaimTypes.NameIdentifier, id), 
                                     new Claim(ClaimTypes.Role, "Foo")
                                 };

                return claims;
            }
            else
            {
                return null;
            }
        }
    }
}