namespace WebApi2_Authentication.Controllers
{
    using System.Collections.Generic;
    using System.Net;
    using System.Net.Http;
    using System.Threading.Tasks;
    using System.Web.Http;
    using WebApi2_Authentication.Models;
    using WebApi2_Authentication.Models.Repository;

    public class WebApiController : ApiController
    {
        private IProductRepository productRepository;

        public WebApiController()
        {
            this.productRepository = new ProductRepository();
        }

        // Returning void (No Result)
        public void Delete(int id)
        {
            // This will return 204 No content http response
            this.productRepository.DeleteProductAsync(id);
        }

        public async Task DeleteProduct(int id)
        {
            await this.productRepository.DeleteProductAsync(id); // send in id of product obviously
        }

        // Ok() method will create OkResult object and return it to client
        [HttpGet]
        [Route("api/products/noop/")]
        public IHttpActionResult NoOp()
        {
            return this.Ok();
        }

        // Creating StatusCodeResult object explicitly
        public IHttpActionResult Delete2(int id)
        {
            this.productRepository.DeleteProductAsync(id);
            return this.StatusCode(HttpStatusCode.NoContent);
        }

        // Creating HttpResponseObject yourself
        public IHttpActionResult Delete3(int id)
        {
            this.productRepository.DeleteProductAsync(id);
            return this.ResponseMessage(new HttpResponseMessage(HttpStatusCode.NoContent));
        }

        // Creating custom ActionResult object => CustomNoContentResult.cs
        public IHttpActionResult Delete5(int id)
        {
            this.productRepository.DeleteProductAsync(id);
            return new CustomNoContentResult();
        }

        // Returning Model data
        [Route("api/products/GetProduct/")]
        public IEnumerable<Product> GetAll()
        {
            return this.productRepository.Products();
        }

        // Returning Action Results OR Objects - Same thing
        [Route("api/products/GetProduct2/")]
        public IHttpActionResult GetProduct(int id)
        {
            // Getting Identity info stuff
            Product product = this.productRepository.GetProduct();
            return (product == null) ? (IHttpActionResult)this.BadRequest("No product found") : this.Ok("everything ok");
        }

        // Returning Action Results OR Objects - Same thing
        [Route("api/products/GetProduct3/")]
        public Product GetProduct2(string id)
        {
            // string for it to build
            Product product = this.productRepository.GetProduct();
            if (product == null)
            {
                throw new HttpResponseException(HttpStatusCode.BadRequest);
            }

            return product;
        }

        public async Task<IHttpActionResult> PostProduct(Product product)
        {
            if (this.ModelState.IsValid)
            {
                // save in repo
                await this.productRepository.SaveProductAsync(product);
                return this.Ok();
            }

            // returns bad request
            return this.BadRequest(this.ModelState);
        }
    }
}