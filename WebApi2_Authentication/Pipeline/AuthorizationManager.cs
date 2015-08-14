namespace WebApi2_Authentication.Pipeline
{
    using System.Security.Claims;

    // This checks claims - access level
    public class AuthorizationManager : ClaimsAuthorizationManager
    {
        public override bool CheckAccess(AuthorizationContext context)
        {
            return true;
        }
    }
}