namespace WebApi2_Authentication
{
    using System.Diagnostics;
    using System.Security.Principal;

    public class HttpModuleHelper
    {
        public static void Print(string stage, IPrincipal principal)
        {
            if (principal == null || principal.Identity == null || !principal.Identity.IsAuthenticated)
            {
                Debug.WriteLine(stage + " : " + "Anonymous user");
            }
            else
            {
                Debug.WriteLine(stage + " : " + principal.Identity.Name);
            }
        }
    }
}