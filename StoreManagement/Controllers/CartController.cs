using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        [HttpGet]
        public IActionResult GetCart(string ProductIdentifiers) {
            var ProductDictionary = OrderHelper.GetProductDictionary(ProductIdentifiers);
            return Ok(ProductDictionary);
        
        }
    }
}
