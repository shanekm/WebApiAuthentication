namespace WebApi2_Authentication
{
    using System.Web.Http;
    using System.Web.Http.Controllers;

    public class TestAuthorizationFilterAttribute : AuthorizeAttribute
    {
        protected override bool IsAuthorized(HttpActionContext actionContext)
        {
            HttpModuleHelper.Print("TestAuthorizationFilter", actionContext.RequestContext.Principal);

            return base.IsAuthorized(actionContext);
        }
    }
}