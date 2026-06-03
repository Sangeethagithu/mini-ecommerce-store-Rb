using Microsoft.AspNetCore.Http;
namespace Amazon.API.Models.DTOs.Product
{
    public class CreateProductDto
    {
        public string Name { get; set; }

        public string Description { get; set; }

        public decimal Price { get; set; }

        public int StockQuantity { get; set; }

        public IFormFile Image { get; set; } //image file from client

        public Guid CategoryId { get; set; }
    }
}