namespace WebApi2_Authentication
{
    using System;
    using System.Web;

    public class HttpModule : IHttpModule
    {
        public void Dispose()
        {
        }

        public void Init(HttpApplication context)
        {
            context.BeginRequest += this.context_BeginRequest;
        }

        private void context_BeginRequest(object sender, EventArgs e)
        {
            HttpModuleHelper.Print("HttpModule: ", HttpContext.Current.User);
        }
    }
}