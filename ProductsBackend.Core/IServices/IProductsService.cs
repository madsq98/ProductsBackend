using System.Collections.Generic;
using ProductsBackend.Core.Models;

namespace ProductsBackend.Core.IServices
{
    public interface IProductsService
    {
        public List<Product> GetAllProducts();

        public Product AddProduct(Product product);
        public Product GetOneProduct(int productId);
        public Product UpdateProduct(int productId, Product returnProduct);
        public Product DeleteProduct(Product returnProduct);
    }
}