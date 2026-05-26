using Amazon.API.Models.DTOs.Category;
using Amazon.API.Repositories;
using Microsoft.AspNetCore.Mvc;

namespace Amazon.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CategoriesController : ControllerBase
    {
        private readonly CategoryRepository categoryRepository;

        public CategoriesController(CategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }

        [HttpGet]   //get operation api endpoint
        public IActionResult GetAllCategories()
        {
            var categories = categoryRepository.GetAllCategories();

            return Ok(categories);
        }

        [HttpPost] //post operation
        public IActionResult AddCategory(CreateCategoryDto dto)
        {
            categoryRepository.AddCategory(dto);

            return Ok("Category added successfully");
        }
        //update 
        [HttpPut("{id}")]
        public IActionResult UpdateCategory(Guid id, UpdateCategoryDto dto)
        {
            categoryRepository.UpdateCategory(id, dto);

            return Ok("Category updated successfully");
        }
        //delete 
        [HttpDelete("{id}")]
        public IActionResult DeleteCategory(Guid id)
        {
            categoryRepository.DeleteCategory(id);

            return Ok("Category deleted successfully");
        }
    }
}