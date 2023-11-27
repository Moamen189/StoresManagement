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

        private readonly List<string> productsList = new List<string>()
        {
            "Phones" , "Computers" , "Printers" , "Accessiores" , "Cameras" , "Other"
        };

        public ProductsController(ApplicationDbContext applicationDbContext , IWebHostEnvironment env)
        {
            this.context = applicationDbContext;
            this.env = env;
        }
        [HttpGet("{categories}")]

        public IActionResult GetCategory()
        {
            return Ok(productsList);
        }
        [HttpGet]

        public IActionResult GetProducts(string? search , string? category , int? maxPrice , int? minPrice) {
            IQueryable<Product> query = context.Products;
            if(search != null)
            {
                query = query.Where(p => p.Name.Contains(search) || p.Description.Contains(search));
            }
            if (category != null)
            {
                query = query.Where(p => p.Category == category);
            }
            if(minPrice != null)
            {
                query = query.Where(p => p.Price >= minPrice);
            }
            if (maxPrice != null)
            {
                query = query.Where(p => p.Price <= maxPrice);
            }
            var Products = query.ToList();


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
            if (!productsList.Contains(productDto.Category))
            {

                ModelState.AddModelError("Category", "Please Select a valid Category");
                return BadRequest(ModelState);

            }
            if (productDto.ImageFileName == null) {

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

        [HttpPut("{id}")]

        public IActionResult UpdateProduct(int id,[FromForm]ProductDto productDto) {

            if (!productsList.Contains(productDto.Category))
            {

                ModelState.AddModelError("Category", "Please Select a valid Category");
                return BadRequest(ModelState);

            }

            var Product = context.Products.Find(id);
            if (Product == null)
            {
                return NotFound();
            }
            string productImageFileName = Product.ImageFileName;
            if(productDto.ImageFileName != null)
            {
                string ImageFileName = DateTime.Now.ToString("yyyyMMddHHmmssfff");
                ImageFileName += Path.GetExtension(productDto.ImageFileName.FileName);
                string ImageFolder = env.WebRootPath + "/Images/Products/";
                using (var stream = System.IO.File.Create(ImageFolder + ImageFileName))
                {
                    productDto.ImageFileName.CopyTo(stream);
                }

                System.IO.File.Delete(ImageFolder + Product.ImageFileName);
            }
            Product.Name = productDto.Name;
            Product.Brand = productDto.Brand;
            Product.Category = productDto.Category;
            Product.Price = productDto.Price;
            Product.Description = productDto.Description ?? "";
            Product.ImageFileName = productImageFileName;
            context.SaveChanges();
            return Ok(Product);
        }


        [HttpDelete("{id}")]

        public IActionResult DeleteProduct(int id)
        {
            var product = context.Products.Find(id);
            if (product == null) { 
                return NotFound();
            }

            var imageFolder = env.WebRootPath + "/Images/Products/";
            System.IO.File.Delete(imageFolder + product.ImageFileName);
            context.Products.Remove(product);
            context.SaveChanges();
            return Ok();

        }
    }
}
