
using Amazon.API.Models.DTOs.Product;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
namespace Amazon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository productRepository;

        public ProductsController(ProductRepository productRepository)
        {
            this.productRepository = productRepository;
        }

        //get operation

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            var products = productRepository.GetAllProducts();

            return Ok(products);
        }

        //post operation

        //http request sent data to server


        [Authorize(Roles = "Admin")]
        [HttpPost]

        public IActionResult AddProduct(CreateProductDto dto)
        {
            productRepository.AddProduct(dto);

            return Ok("Product added successfully");//senting response back to client
        }

        //updating PUT id comes from url
        [Authorize(Roles = "Admin")]
        [HttpPut("{id}")]
        public IActionResult UpdateProduct(Guid id, UpdateProductDto dto)
        {
            productRepository.UpdateProduct(id, dto);

            return Ok("Product updated successfully");
        }


        //delete with id
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            productRepository.DeleteProduct(id);

            return Ok("Product deleted successfully");
        }


        //product search
        [HttpGet("search")]
        public IActionResult SearchProducts(string name)
        {
            var products = productRepository.SearchProducts(name);

            return Ok(products);
        }

        //restocking products
        [Authorize(Roles = "Admin")]
        [HttpPut("stock")]
        public IActionResult UpdateStock(
    UpdateStockDto dto)
        {
            productRepository.UpdateStock(dto);

            return Ok(
                "Product stock updated successfully");
        }
        //low stock alert
        [HttpGet("low-stock")]
        public IActionResult GetLowStockProducts()
        {
            var products =
                productRepository.GetLowStockProducts();

            return Ok(products);
        }
    }
}