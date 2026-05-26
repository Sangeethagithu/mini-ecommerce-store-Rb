using Amazon.API.Models.DTOs.Order;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
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
    }
}