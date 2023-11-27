using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public ProductsController(ApplicationDbContext applicationDbContext)
        {
            this.context = applicationDbContext;
        }

        [HttpGet]

        public IActionResult GetProducts() {
          var Products = context.Products.ToList();
            return Ok(Products);
        
        }

        [HttpGet("{id}")]

        public IActionResult GetProduct(int id) {
            var product = context.Products.Find(id);
            if (product == null)
            {
                return NotFound();
            }
            return Ok(product);
        
        
        }
    }
}
