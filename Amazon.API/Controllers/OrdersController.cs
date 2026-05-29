using Amazon.API.Models.DTOs.Order;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Amazon.API.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class OrdersController : ControllerBase
    {
        private readonly OrderRepository orderRepository;

        public OrdersController(
            OrderRepository orderRepository)
        {
            this.orderRepository =
                orderRepository;
        }

        [HttpPost("checkout")]
        public IActionResult Checkout(
            CheckoutDto dto)
        {
            orderRepository.Checkout(dto);

            return Ok("Order placed successfully");
        }

        //order status 
        [HttpPut("status")]
        public IActionResult UpdateOrderStatus(
    UpdateOrderStatusDto dto)
        {
            orderRepository.UpdateOrderStatus(dto);

            return Ok("Order status updated successfully");
        }

        //to get prev orders of user
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            var orders =
                orderRepository.GetAllOrders();

            return Ok(orders);
        }
        //give prod in order
        [HttpGet("{orderId}")]
        public IActionResult GetOrderDetails(
    Guid orderId)
        {
            var items =
                orderRepository.GetOrderDetails(
                    orderId);

            return Ok(items);
        }
    }
}