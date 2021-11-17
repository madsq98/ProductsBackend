using Microsoft.EntityFrameworkCore;
using ProductsBackend.EntityCore.Entities;

namespace ProductsBackend.EntityCore
{
    public class ProductsContext : DbContext
    {
        public DbSet<ProductEntity> Products { get; set; }

        public ProductsContext(DbContextOptions<ProductsContext> options) : base(options)
        {
            
        }
    }
}