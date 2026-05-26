using Amazon.API.Models.DTOs.Cart;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CartController : ControllerBase
    {
        private readonly CartRepository cartRepository;

        public CartController(CartRepository cartRepository)
        {
            this.cartRepository = cartRepository;
        }

        [HttpPost("add")]//attribute added to specify the route for adding items to the cart
        public IActionResult AddToCart(AddToCartDto dto)
        {
            cartRepository.AddToCart(dto);

            return Ok("Product added to cart successfully");
        }
        [HttpGet("{cartId}")] //attribute added to specify the route for getting cart items
        public IActionResult GetCartItems(Guid cartId)
        {
            var items = cartRepository.GetCartItems(cartId);

            return Ok(items);
        }

        [HttpPut("update")] //attribute added to specify the route for updating cart quantity
        public IActionResult UpdateCartQuantity(
    UpdateCartQuantityDto dto)
        {
            cartRepository.UpdateCartQuantity(dto);

            return Ok("Cart quantity updated successfully");
        }

        [HttpDelete("{cartItemId}")]//remove cart item
        public IActionResult RemoveCartItem(Guid cartItemId)
        {
            cartRepository.RemoveCartItem(cartItemId);
            
            return Ok("Cart item removed successfully");
        }

        //total sum in cart
        [HttpGet("total/{cartId}")]
        public IActionResult GetCartTotal(Guid cartId)
        {
            var total = cartRepository.GetCartTotal(cartId);

            return Ok(total);
        }
    }
}