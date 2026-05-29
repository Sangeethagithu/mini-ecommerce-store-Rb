namespace Amazon.API.Models.DTOs.Order
{
    public class UpdateOrderStatusDto
    {
        public Guid OrderId { get; set; }

        public string Status { get; set; }
    }
}