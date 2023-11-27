using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : ControllerBase
    {
        private readonly ApplicationDbContext context;
        private readonly IWebHostEnvironment env;

        public ProductsController(ApplicationDbContext applicationDbContext , IWebHostEnvironment env)
        {
            this.context = applicationDbContext;
            this.env = env;
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

        [HttpPost]

        public IActionResult CreateProduct([FromForm]ProductDto productDto)
        {
            if(productDto.ImageFileName == null) {

                ModelState.AddModelError("Image File", "the Image File Name is Required");
                return BadRequest(ModelState);
            
            }

            string ImageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
            ImageFileName += Path.GetExtension(productDto.ImageFileName.FileName);
            string ImageFolder = env.WebRootPath + "/Images/Products/";
            using(var stream = System.IO.File.Create(ImageFolder+ ImageFileName))
            {
                productDto.ImageFileName.CopyTo(stream);
            }

            Product product = new Product()
            {
                Name = productDto.Name,
                Brand = productDto.Brand,
                Category = productDto.Category,
                Price = productDto.Price,
                Description = productDto.Description ?? "",
                ImageFileName = ImageFileName,
                CreatedAt = DateTime.Now,
            };

            context.Products.Add(product);
            context.SaveChanges();
            return Ok(product);
        }
    }
}
