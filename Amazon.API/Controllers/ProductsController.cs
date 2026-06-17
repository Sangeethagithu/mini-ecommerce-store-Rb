
using Amazon.API.Models.DTOs.Product;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Authorization;
using Microsoft.Extensions.Logging;
namespace Amazon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductsController : ControllerBase
    {
        private readonly ProductRepository productRepository;

        

        private readonly ILogger<ProductsController> logger;

        public ProductsController(
      ProductRepository productRepository,
      ILogger<ProductsController> logger)
        {
            this.productRepository =
                productRepository;

            this.logger = logger;
        }

        //get operation

        [HttpGet]
        public IActionResult GetAllProducts()
        {
            
            logger.LogInformation(
       "Fetching all products.");
          
            var products = productRepository.GetAllProducts();

            return Ok(products);
        }

        //post operation

        //http request sent data to server


        [Authorize(Roles = "Admin")]
        [HttpPost]
        [Consumes("multipart/form-data")]
        public IActionResult AddProduct(
     [FromForm] CreateProductDto dto)
        {


            logger.LogInformation(
       "Fetching all products.");
            try
            {
                productRepository.AddProduct(dto);

                return Ok("Product Added Successfully");
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }

        //updating PUT id comes from url
        
        

        [HttpPut("{id}")]
        [Consumes("multipart/form-data")]
        public IActionResult UpdateProduct(
    Guid id,
    [FromForm] UpdateProductDto dto)
        {
            productRepository.UpdateProduct(id, dto);

            return Ok("Product updated successfully");
        }
        [HttpGet("{id}")]
        public IActionResult GetProductById(Guid id)
        {
            var product =
                productRepository.GetProductById(id);

            return Ok(product);
        }

        //delete with id
        [Authorize(Roles = "Admin")]
        [HttpDelete("{id}")]
        public IActionResult DeleteProduct(Guid id)
        {
            logger.LogInformation(
    "Deleting product {ProductId}",
    id);
            productRepository.DeleteProduct(id);

            return Ok("Product deleted successfully");
        }


        //product search
        [HttpGet("search")]
        public IActionResult SearchProducts(string name)
        {
            logger.LogInformation(
    "Searching products: {SearchText}",
    name);
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