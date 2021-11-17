using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ProductsBackend.Core.IServices;
using ProductsBackend.Core.Models;
using ProductsBackend.CoreWebAPI.DTO;

namespace ProductsBackend.CoreWebAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly IProductsService _service;

        public ProductsController(IProductsService service)
        {
            _service = service;
        }

        [HttpGet("{id}")]
        public ActionResult<ProductOut> GetOne(int id)
        {
            Product result = _service.GetOneProduct(id);
            return Ok(new ProductOut {Id = result.Id, Name = result.Name});
        }
        
        [HttpGet]
        public ActionResult<ProductsOut> GetAll()
        {
            List<Product> result = _service.GetAllProducts();
            List<ProductOut> returnResult = result.Select(x => new ProductOut
            {
                Id = x.Id,
                Name = x.Name
            }).ToList();
            return Ok(new ProductsOut
            {
                list = returnResult
            });
        }

        [HttpPost]
        public ActionResult<ProductOut> Add([FromBody] ProductIn productIn)
        {
            Product result = _service.AddProduct(new Product
            {
                Name = productIn.name
            });
            ProductOut returnResult = new ProductOut
            {
                Id = result.Id,
                Name = result.Name
            };

            return Ok(returnResult);
        }

        [HttpPut("{id}")]
        public ActionResult<ProductOut> Update(int id, [FromBody] ProductOut productIn)
        {
            Product result = _service.UpdateProduct(id, new Product { Id = productIn.Id, Name = productIn.Name });
            return Ok(new ProductOut {Id = result.Id, Name = result.Name});
        }
    }
}