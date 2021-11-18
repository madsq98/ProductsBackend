using System.Collections.Generic;
using System.Linq;
using ProductsBackend.Core.Models;
using ProductsBackend.Domain.IRepositories;
using ProductsBackend.EntityCore.Entities;

namespace ProductsBackend.EntityCore.Repositories
{
    public class ProductsRepository : IRepository<Product> 
    {
        private readonly ProductsContext _ctx;

        public ProductsRepository(ProductsContext ctx)
        {
            _ctx = ctx;
        }
        public List<Product> GetAll()
        {
            return ConvertProducts().ToList();
        }

        public Product Add(Product obj)
        {
            var newEntity = _ctx.Products.Add(new ProductEntity
            {
                Id = obj.Id,
                Name = obj.Name
            }).Entity;
            _ctx.SaveChanges();

            obj.Id = newEntity.Id;
            return obj;
        }

        public Product GetOne(int id)
        {
            return ConvertProducts().FirstOrDefault(p => p.Id == id);
        }

        public Product Update(int id, Product obj)
        {

            ProductEntity newEntity = new ProductEntity
            {
                Id = obj.Id,
                Name = obj.Name
            };

            _ctx.Products.Update(newEntity);
            _ctx.SaveChanges();

            obj.Id = id;

            return obj;
        }

        public Product Delete(Product obj)
        {
            ProductEntity entity = new ProductEntity {Id = obj.Id};

            _ctx.Products.Remove(entity);
            _ctx.SaveChanges();

            return obj;
        }

        //Helper methods
        private IQueryable<Product> ConvertProducts()
        {
            return _ctx.Products.Select(entity => new Product
            {
                Id = entity.Id,
                Name = entity.Name
            });
        }
    }
}