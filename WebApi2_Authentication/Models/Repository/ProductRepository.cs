namespace WebApi2_Authentication.Models.Repository
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;

    public class ProductRepository : IProductRepository
    {
        public ProductRepository()
        {
            this.products = new List<Product>
                                {
                                    new Product { Id = 1, Name = "Monitor" }, 
                                    new Product { Id = 2, Name = "Computer" }
                                };
        }

        public List<Product> products { get; set; }

        public IEnumerable<Product> Products()
        {
            return this.products;
        }

        public async Task<int> SaveProductAsync(Product product)
        {
            if (product.Id == 0)
            {
                // add new product
                this.products.Add(product);
            }
            else
            {
                this.products.Find(x => x.Id == product.Id);

                // do update etc
            }

            await Task.Delay(10000);
            return 1;
        }

        public async Task<int> DeleteProductAsync(int id)
        {
            // find and remove product
            await Task.Delay(10000);
            return id;
        }

        public Product GetProduct()
        {
            return this.products.FirstOrDefault();
        }
    }
}