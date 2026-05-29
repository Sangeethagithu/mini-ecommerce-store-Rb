namespace Amazon.API.Models.DTOs.Product
{
    public class UpdateStockDto
    {
        public Guid ProductId { get; set; }

        public int StockQuantity { get; set; }
    }
}