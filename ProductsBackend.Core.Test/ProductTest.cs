using ProductsBackend.Core.Models;
using Xunit;

namespace ProductsBackend.Core.Test
{
    public class ProductTest
    {
        [Fact]
        public void Product_Exists()
        {
            var product = new Product();

            Assert.NotNull(product);
        }

        [Fact]
        public void Product_HasIntProperty_Id()
        {
            var product = new Product();
            product.Id = (int) 1;
            Assert.Equal(1, product.Id);
        }

        [Fact]
        public void Product_HasStringProperty_Name()
        {
            var product = new Product();
            product.Name = (string) "Test";
            Assert.Equal("Test", product.Name);
        }
    }
}