namespace WebApi2_Authentication
{
    using System;
    using System.Collections.Generic;
    using System.Security.Principal;
    using System.Threading.Tasks;
    using Microsoft.Owin;

    public class TestOwinMiddleware
    {
        private Func<IDictionary<string, object>, Task> _next;

        public TestOwinMiddleware(Func<IDictionary<string, object>, Task> next)
        {
            this._next = next;
        }

        public async Task Invoke(IDictionary<string, object> env)
        {
            var context = new OwinContext(env);

            // Setting Identity => roles go in string[]
            context.Request.User = new GenericPrincipal(new GenericIdentity("someUser"), new string[] { });

            HttpModuleHelper.Print("Middleware", context.Request.User);

            await this._next(env); // call next thing in the pipeline passing envirenment
        }
    }
}