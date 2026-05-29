namespace Amazon.API.Models.DTOs.Order
{
    public class OrderItemDto
    {
        public string ProductName { get; set; }

        public int Quantity { get; set; }

        public decimal Price { get; set; }
    }
}