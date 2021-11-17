using System.Collections.Generic;
using Moq;
using ProductsBackend.Core.Models;
using ProductsBackend.Domain.IRepositories;
using ProductsBackend.Domain.Services;
using Xunit;

namespace ProductsBackend.Core.Test
{
    public class ProductServiceTest
    {
        [Fact]
        public void ProductService_Exists()
        {
            var repoMock = new Mock<IRepository<Product>>();
            
            var service = new ProductsService(repoMock.Object);
            Assert.NotNull(service);
        }

        [Fact]
        public void ProductService_GetAll_ReturnsList()
        {
            List<Product> testList = new List<Product>();

            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(s => s.GetAll())
                .Returns(testList);

            var service = new ProductsService(repoMock.Object);

            Assert.NotNull(service.GetAllProducts());
        }

        [Fact]
        public void ProductService_Add_ReturnsProduct()
        {
            Product returnProduct = new Product();

            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(s => s.Add(returnProduct))
                .Returns(returnProduct);

            var service = new ProductsService(repoMock.Object);

            Assert.NotNull(service.AddProduct(returnProduct));
        }

        [Fact]
        public void ProductService_GetOne_ReturnsProduct()
        {
            int id = 2;
            Product product = new Product {Id = id};

            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(s => s.GetOne(id))
                .Returns(product);

            var service = new ProductsService(repoMock.Object);

            Assert.NotNull(service.GetOneProduct(id));
        }

        [Fact]
        public void ProductService_Update_ReturnsProduct()
        {
            int id = 2;
            Product product = new Product {Id = 2};
            
            var repoMock = new Mock<IRepository<Product>>();
            repoMock.Setup(s => s.Update(id, product))
                .Returns(product);

            var service = new ProductsService(repoMock.Object);

            Assert.NotNull(service.UpdateProduct(id, product));
        }
    }
}