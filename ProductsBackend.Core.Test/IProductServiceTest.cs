using System.Collections.Generic;
using Moq;
using ProductsBackend.Core.IServices;
using ProductsBackend.Core.Models;
using Xunit;

namespace ProductsBackend.Core.Test
{
    public class IProductServiceTest
    {
        [Fact]
        public void IProductService_Exists()
        {
            var serviceMock = new Mock<IProductsService>();
            Assert.NotNull(serviceMock.Object);
        }

        [Fact]
        public void GetAllProducts_WithNoParams_ReturnListOfProducts()
        {
            var serviceMock = new Mock<IProductsService>();
            serviceMock.Setup(s => s.GetAllProducts())
                .Returns(new List<Product>());

            Assert.NotNull(serviceMock.Object.GetAllProducts());
        }

        [Fact]
        public void GetOneProduct_ReturnProduct()
        {
            int productId = 1;
            Product returnProduct = new Product {Id = productId, Name = "Test "};
            
            var serviceMock = new Mock<IProductsService>();
            serviceMock.Setup(s => s.GetOneProduct(productId))
                .Returns(returnProduct);

            Assert.Equal(returnProduct, serviceMock.Object.GetOneProduct(productId));
        }

        [Fact]
        public void UpdateProduct_ReturnProduct()
        {
            int productId = 1;
            Product returnProduct = new Product {Id = productId, Name = "Test "};
            
            var serviceMock = new Mock<IProductsService>();
            serviceMock.Setup(s => s.UpdateProduct(productId, returnProduct))
                .Returns(returnProduct);

            Assert.Equal(returnProduct, serviceMock.Object.UpdateProduct(productId, returnProduct));
        }

        [Fact]
        public void DeleteProduct_ReturnProduct()
        {
            int productId = 1;
            Product returnProduct = new Product {Id = productId, Name = "test"};

            var serviceMock = new Mock<IProductsService>();
            serviceMock.Setup(s => s.DeleteProduct(returnProduct))
                .Returns(returnProduct);

            Assert.Equal(returnProduct, serviceMock.Object.DeleteProduct(returnProduct));
        }
    }
}