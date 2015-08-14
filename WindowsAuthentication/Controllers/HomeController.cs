using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace WindowsAuthentication.Controllers
{
    using System.Diagnostics;
    using System.Net.Http;
    using System.Security.Claims;
    using System.Linq;

    public class HomeController : Controller
    {
        [Authorize]
        public ActionResult Index()
        {
            Debug.Write("AuthenticationType:" + User.Identity.AuthenticationType);
            Debug.Write("IsAuthenticated:" + User.Identity.IsAuthenticated);
            Debug.Write("Name:" + User.Identity.Name);

            var principal = User as ClaimsPrincipal;

            var result = from c in principal.Claims select new 
                         {
                             Type = c.Type,
                             Value = c.Value
                         };

            return this.View();
        }

        // Client calling web api
        public void Client()
        {
            // Send along windows credentials
            var handler = new HttpClientHandler { UseDefaultCredentials = true };

            // Pass in hadler credentials
            var client = new HttpClient(handler) { BaseAddress = new Uri("http://localhost:50271//api/WebApiIdentity/Get") };

            var response = client.GetAsync("identity").Result;
            response.EnsureSuccessStatusCode();

            var content = response.Content.ReadAsStringAsync().Result;
            Debug.Write(content);
        }
    }
}