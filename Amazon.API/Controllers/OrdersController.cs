using Amazon.API.Models.DTOs.Order;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


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
        public IActionResult Checkout()
        {
            string email =
                User.FindFirst(
                    ClaimTypes.Email)?.Value!;

            orderRepository.Checkout(email);

            return Ok(
                "Order placed successfully");
        }
        //recent produ
        [AllowAnonymous]

        [HttpGet("recent-orders")]
        public IActionResult GetRecentOrders()
        {
            var orders =
                orderRepository.GetRecentOrders();

            return Ok(orders);
        }
        //admin dashboard
        [AllowAnonymous]

        [HttpGet("dashboard")]
        public IActionResult GetDashboardStats()
        {
            var dashboard =
                orderRepository.GetDashboardStats();

            return Ok(dashboard);
        }
        //order status 
        [HttpPut("status")]
        public IActionResult UpdateOrderStatus(
    UpdateOrderStatusDto dto)
        {
            orderRepository.UpdateOrderStatus(dto);

            return Ok("Order status updated successfully");
        }
        //admin get order
        
        [HttpGet("admin")]
        public IActionResult GetAllOrdersForAdmin()
        {
            var orders =
                orderRepository.GetAllOrdersForAdmin();

            return Ok(orders);
        }
        //to get prev orders of user
        [HttpGet]
        public IActionResult GetAllOrders()
        {
            string email =
                User.FindFirst(
                    ClaimTypes.Email)?.Value!;

            var orders =
                orderRepository.GetAllOrders(
                    email);

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