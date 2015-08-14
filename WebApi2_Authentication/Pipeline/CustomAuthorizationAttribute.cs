namespace WebApi2_Authentication.Pipeline
{
    using System.Net;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class CustomAuthorizationAttribute : AuthorizeAttribute
    {
        private const bool outcome = false;

        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            // retrive principal and check auth
            var principal = actionContext.RequestContext.Principal as ClaimsPrincipal;

            return outcome;
        }

        protected override void HandleUnauthorizedRequest(HttpActionContext actionContext)
        {
            var response = actionContext.Request.CreateErrorResponse(
                HttpStatusCode.Unauthorized,
                "unauthorized custom auth attribute");

            actionContext.Response = response;
        }
    }
}