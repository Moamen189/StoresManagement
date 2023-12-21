using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public CartController(ApplicationDbContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public IActionResult GetCart(string ProductIdentifiers) {
            CartDto cartDto = new CartDto();
            cartDto.CartItems = new List<CartItemDto>();
            cartDto.TotalPrice = 0;
            cartDto.SubTotal = 0;
            cartDto.ShippingFee = OrderHelper.ShippingFee;
            var ProductDictionary = OrderHelper.GetProductDictionary(ProductIdentifiers);


            foreach(var pair in ProductDictionary)
            {
                int productId = pair.Key;
                var Product = context.Products.Find(productId);
                if(Product == null)
                {
                    continue;
                }
                var cartItemDto = new CartItemDto();
                cartItemDto.product = Product;
                cartItemDto.Quantity = pair.Value;
                cartDto.CartItems.Add(cartItemDto);
                cartDto.SubTotal +=Product.Price * pair.Value;
                cartDto.TotalPrice = cartDto.SubTotal + cartDto.ShippingFee;

            }
            return Ok(cartDto);
        
        }
    }
}
