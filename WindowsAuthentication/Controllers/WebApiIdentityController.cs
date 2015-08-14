namespace WindowsAuthentication.Controllers
{
    using System.Collections.Generic;
    using System.Diagnostics;
    using System.Linq;
    using System.Security.Claims;
    using System.Web.Http;

    public class WebApiIdentityController : ApiController
    {
        [Authorize]
        public IEnumerable<ViewClaim> Get()
        {
            Debug.Write("AuthenticationType:" + this.User.Identity.AuthenticationType);
            Debug.Write("IsAuthenticated:" + this.User.Identity.IsAuthenticated);
            Debug.Write("Name:" + this.User.Identity.Name);

            var principal = this.User as ClaimsPrincipal;

            var result = from c in principal.Claims select new ViewClaim{ Type = c.Type, Value = c.Value };
            return result;

            //if (this.User.Identity.IsAuthenticated)
            //{
            //    return this.Ok("Authenticated: " + this.User.Identity.Name + " : " + result);
            //}
            //else
            //{
            //    return this.BadRequest("Not authenticated");
            //}
        }
    }

    public class ViewClaim
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}