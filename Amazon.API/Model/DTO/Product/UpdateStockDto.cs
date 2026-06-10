using System.ComponentModel.DataAnnotations;

namespace Amazon.API.Models.DTOs.Product
{
    public class UpdateStockDto
    {
        public Guid ProductId { get; set; }

        [Range(1, int.MaxValue,
      ErrorMessage = "Stock must be at least 1")]
        public int StockQuantity { get; set; }
    }
}