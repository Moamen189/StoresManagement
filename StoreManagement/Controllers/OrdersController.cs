using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
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
        [HttpGet]
        public IActionResult GetOrders(int? page)
        {
            int userId = JwtReader.getUserId(User);
            var role = context.Users.Find(userId)?.Role ?? "";
            IQueryable<Order> query = context.Orders.Include(o => o.User).Include(o => o.OrderItems).ThenInclude(o => o.Product);
            if (role != "Admin")
            {
                query = query.Where(o => o.UserId == userId);
            }

            query = query.OrderByDescending(x => x.Id);

            if(page == null || page < 1)
            {
                page = 1;
            }

            int PageSize = 5;
            int totalPages = 0;
            decimal count = query.Count();
            totalPages = (int)Math.Ceiling(count / PageSize);

            query = query.Skip((int)(page -1) * PageSize).Take(PageSize);
            var orders = query.ToList();
            foreach (var order in orders)
            {
                foreach(var item in order.OrderItems) {

                    item.Order = null;
                
                }
                order.User.Password = "";
            }
            var response = new
            {
                Page = page,
                TotalPages = totalPages,
                Orders = orders,
            };
            return Ok(response);
        }
        [Authorize]
        [HttpGet("{id}")]
        public IActionResult GetOrder(int id)
        {
            var userId = JwtReader.getUserId(User);
            var role = context.Users.Find(userId)?.Role ?? "";

            Order? order = null;

            if (role == "admin")
            {
                 order = context.Orders.Include(c=> c.User).Include(v => v.OrderItems).ThenInclude(c=> c.Product).FirstOrDefault(u => u.Id == id);
            }
            else
            {
                order = context.Orders.Include(c => c.User).Include(v => v.OrderItems).ThenInclude(c => c.Product).FirstOrDefault(u => u.Id == id && u.UserId == userId);

            }

            if(order == null)
            {
                return NotFound();
            }

            foreach (var item in order.OrderItems)
            {
                item.Order = null;
            }
            order.User.Password = "";

            return Ok(order);
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
            //Fix Recycling Issue
            foreach(var item in order.OrderItems)
            {
                item.Order = null;
            }

            order.User.Password = "";
            return Ok(order);

        }

        [Authorize(Roles ="admin")]
        [HttpPut("{id}")]

        public IActionResult UpdateOrder(int id , string? paymentStatus , string? orderStatus)
        {
            if(orderStatus == null && paymentStatus == null)
            {
                ModelState.AddModelError("Update Order ", "there is nothig to update");
                return BadRequest(ModelState);
            }

            if (paymentStatus != null && !OrderHelper.PaymentStatuses.Contains(paymentStatus))
            {
                ModelState.AddModelError("Payment Status", "Payment Status is not valid");
                return BadRequest(ModelState);
            }


            if (orderStatus != null && !OrderHelper.PaymentStatuses.Contains(orderStatus))
            {
                ModelState.AddModelError("Order Status", "Order Status is not valid");
                return BadRequest(ModelState);
            }

            var order = context.Orders.Find(id);
            if (paymentStatus != null)
            {
                order.PaymentStatus = paymentStatus;
            }

            if (orderStatus != null)
            {
                order.OrderStatus = orderStatus;
            }

            context.SaveChanges();

            return Ok(order);


        }
    }
}
