

using System.ComponentModel.DataAnnotations;

namespace Amazon.API.Models.DTOs.Product
{
    public class UpdateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        [Range(1, int.MaxValue,
       ErrorMessage = "Stock must be at least 1")]
        public int StockQuantity { get; set; }
        public IFormFile? Image { get; set; } //image file


        public Guid CategoryId { get; set; }
    }
}