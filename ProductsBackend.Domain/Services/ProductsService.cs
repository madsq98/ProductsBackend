using System.Collections.Generic;
using ProductsBackend.Core.IServices;
using ProductsBackend.Core.Models;
using ProductsBackend.Domain.IRepositories;

namespace ProductsBackend.Domain.Services
{
    public class ProductsService : IProductsService
    {
        private readonly IRepository<Product> _repo;

        public ProductsService(IRepository<Product> repo)
        {
            _repo = repo;
        }
        
        public List<Product> GetAllProducts()
        {
            return _repo.GetAll();
        }

        public Product AddProduct(Product product)
        {
            return _repo.Add(product);
        }

        public Product GetOneProduct(int productId)
        {
            return _repo.GetOne(productId);
        }

        public Product UpdateProduct(int productId, Product returnProduct)
        {
            return _repo.Update(productId, returnProduct);
        }

        public Product DeleteProduct(Product returnProduct)
        {
            return _repo.Delete(returnProduct);
        }
    }
}