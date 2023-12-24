using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using StoreManagement.Models;
using StoreManagement.Services;

namespace StoreManagement.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        private readonly ApplicationDbContext context;

        public OrdersController(ApplicationDbContext context)
        {
            this.context = context;
        }

        [Authorize]
        [HttpPost]
        public IActionResult CreateOrder(OrderDto orderDto)
        {

            if(!OrderHelper.PaymentMethods.ContainsKey(orderDto.PaymentMethod))
            {
                ModelState.AddModelError("Payment Method", "Please Select a valid Payment Method");
                return BadRequest(ModelState);
            }

            int userId = JwtReader.getUserId(User);

            var user = context.Users.Find(userId);
            if (user == null)
            {
                ModelState.AddModelError("Order", "Unable to create new Order");
                return BadRequest(ModelState);
            }

            var productDictionary = OrderHelper.GetProductDictionary(orderDto.ProductIdentifiers);

            Order order = new Order();

            order.UserId = userId;
            order.CreatedAt = DateTime.UtcNow;
            order.ShippingFee = OrderHelper.ShippingFee;
            order.DeliveryMethod = orderDto.DeliveryAddress;
            order.PaymenytMethod = orderDto.PaymentMethod;
            order.OrderStatus = OrderHelper.OrderStatuses[0];
            order.PaymentStatus = OrderHelper.PaymentStatuses[0];

            foreach (var pair in productDictionary)
            {
                int productId = pair.Key;
                var product = context.Products.Find(productId);
                if (product == null)
                {
                    ModelState.AddModelError("Product" , "Product with Id :" +  productId + "is unvalid");
                    return BadRequest(ModelState);
                }

                var orderItem = new OrderItem();
                orderItem.ProductId = productId;
                orderItem.Quantity = pair.Value;
                orderItem.unitPrice = product.Price;

                order.OrderItems.Add(orderItem);
            }

            if(order.OrderItems.Count < 1)
            {
                ModelState.AddModelError("Order", "Unable to create Order");
                return BadRequest(ModelState);
            }

            context.Orders.Add(order);
            context.SaveChanges();
            return Ok(order);

        }
    }
}
