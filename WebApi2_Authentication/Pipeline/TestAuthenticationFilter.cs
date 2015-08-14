namespace WebApi2_Authentication
{
    using System;
    using System.Threading;
    using System.Threading.Tasks;
    using System.Web.Http.Filters;

    public class TestAuthenticationFilterAttribute : Attribute, IAuthenticationFilter
    {
        public async Task AuthenticateAsync(HttpAuthenticationContext context, CancellationToken cancellationToken)
        {
            HttpModuleHelper.Print("Authentication Filter", context.ActionContext.RequestContext.Principal);
        }

        public async Task ChallengeAsync(
            HttpAuthenticationChallengeContext context, 
            CancellationToken cancellationToken)
        {
            // throw new NotImplementedException();
        }

        public bool AllowMultiple
        {
            get
            {
                return false;
            }
        }
    }
}