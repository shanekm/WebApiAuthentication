namespace BrowserBasedAuthentication.Controllers
{
    using System.Net;
    using System.Net.Http;
    using System.Web.Http;
    using BrowserBasedAuthentication.Filters;

    public class WebApiController : ApiController
    {
        [ValidateAntiForgeryToken]
        [HttpGet]
        [Route("api/products/noop/")]
        public HttpResponseMessage Post()
        {
            // some work
            return Request.CreateResponse(HttpStatusCode.Accepted);
        }

        // Javascript 
        // Retrieve token from view and pass it in headers which will be validated above
        //var csrfToken = $("input[name='__RequestVerificationToken']").val(); 
        //$.ajax({
        //    headers: { __RequestVerificationToken: csrfToken },
        //    type: "POST",
        //    dataType: "json",
        //    contentType: 'application/json; charset=utf-8',
        //    url: "/api/products",
        //    data: JSON.stringify({ name: "Milk", price: 2.33 }),
        //    statusCode: {
        //        200: function () {
        //            alert("Success!");
        //        }
        //    }
        //});

    }
}