namespace WebApi2_Authentication.Controllers
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;

    using WebApi2_Authentication.Pipeline;

    public class WebApiIdentityController : ApiController
    {
        // Adding authentication and authorization
        [TestAuthenticationFilter]
        [TestAuthorizationFilter]
        public IHttpActionResult Get()
        {
            // Get current user
            var user1 = this.User;
            HttpModuleHelper.Print("Controller", user1);

            // getting user from context
            var user2 = RequestContext.Principal;
            HttpModuleHelper.Print("Controller", user2);

            return this.Ok();
        }

        [Authorize]
        [CustomAuthorization]
        [HttpGet]
        [Route("api/identity/noop/")]
        public IEnumerable<ViewClaim> NoOp()
        {
            var principal = Request.GetRequestContext().Principal;

            if (principal.Identity.IsAuthenticated)
            {
                return new[] { new ViewClaim { Type = "status", Value = "anonymous" } };
            }

            var principal2 = this.User as ClaimsPrincipal;
            var result = from c in principal2.Claims select new ViewClaim { Type = c.Type, Value = c.Value };


            return result;
        }
    }


    public class ViewClaim
    {
        public string Type { get; set; }

        public string Value { get; set; }
    }
}