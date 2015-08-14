namespace WebApi2_Authentication.Models.Repository
{
    using System.Collections.Generic;
    using System.Threading.Tasks;

    public interface IProductRepository
    {
        IEnumerable<Product> Products();

        Task<int> SaveProductAsync(Product product);

        Task<int> DeleteProductAsync(int id);

        Product GetProduct();
    }
}