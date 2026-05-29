namespace Amazon.API.Models.DTOs.Product
{
    public class LowStockProductDto
    {
        public Guid Id { get; set; }

        public string Name { get; set; }

        public int StockQuantity { get; set; }
    }
}