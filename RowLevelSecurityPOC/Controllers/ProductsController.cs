using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using RowLevelSecurityPOC.Models;

namespace RowLevelSecurityPOC.Controllers
{
    [Route("api/[controller]")]
    public class ProductsController : Controller
    {
        private DataContext dataContext;
        public ProductsController(DataContext dataContext)
        {
            this.dataContext = dataContext;
        }

        [HttpGet]
        public IActionResult Get()
        {
            return Ok(dataContext.Products.ToList());
        }

        [HttpGet("{productId}", Name = "ProductGet")]
        public IActionResult Get(Guid productId)
        {
            var product = dataContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (product == null) return NotFound();

            return Ok(product);
        }

        [HttpPost]
        public IActionResult Post([FromBody]Product product)
        {
            product.ProductId = Guid.NewGuid();
            dataContext.Products.Add(product);
            dataContext.SaveChanges();

            var url = Url.Link("ProductGet", new { productId = product.ProductId });
            return Created(url, product);
        }

        public IActionResult Put(Guid productId, [FromBody]Product product)
        {
            var existingProduct = dataContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (existingProduct == null) return NotFound();

            existingProduct.ProductName = product.ProductName;
            existingProduct.UnitPrice = product.UnitPrice;
            existingProduct.UnitsInStock = product.UnitsInStock;
            existingProduct.UnitsOnOrder = product.UnitsOnOrder;
            existingProduct.ReorderLevel = product.ReorderLevel;
            existingProduct.Discontinued = product.Discontinued;

            dataContext.SaveChanges();

            return Ok(existingProduct);
        }

        [HttpDelete("{productId}")]
        public IActionResult Delete(Guid productId, [FromBody]Product product)
        {
            var existingProduct = dataContext.Products.Where(p => p.ProductId == productId).FirstOrDefault();
            if (existingProduct == null) return NotFound();

            dataContext.Products.Remove(existingProduct);

            dataContext.SaveChanges();

            return NoContent();
        }
    }
}