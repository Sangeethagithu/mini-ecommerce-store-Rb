namespace Amazon.API.Models.DTOs.Cart
{
    public class UpdateCartQuantityDto
    {
        public Guid CartItemId { get; set; }

        public int Quantity { get; set; }
    }
}