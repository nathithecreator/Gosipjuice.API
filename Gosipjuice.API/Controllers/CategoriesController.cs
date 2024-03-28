using Gosipjuice.API.Data;
using Gosipjuice.API.Models.Domain;
using Gosipjuice.API.Models.DTO;
using Gosipjuice.API.Repositories.Implementation;
using Gosipjuice.API.Repositories.Interface;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Gosipjuice.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        private readonly ICategoryRepository categoryRepository;

        public CategoriesController(ICategoryRepository categoryRepository)
        {
            this.categoryRepository = categoryRepository;
        }
        //
        [HttpPost]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> CreateCategory(CreateCategoryRequestDto request)
        {
            //Map DTO to Domain Model
            var category = new Category
            {
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

            await categoryRepository.CreateAsync(category);


            //Domain model to Dto
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(category);



        }

        // GET:  https://localhost:7183/api/categories
        [HttpGet]
        
        public async Task<IActionResult> GetAllCategories()
        {
            var categories = await categoryRepository.GetAllAsync();

            var response = new List<CategoryDto>();
            //Map Domain model to DTO
            foreach (var category in categories)
            {
                response.Add(new CategoryDto
                {
                    Id = category.Id,
                    Name = category.Name,
                    UrlHandle = category.UrlHandle
                });
            }

            return Ok(response);
        }

        //GET: https://localhost:7183/api/categories/{id}
        [HttpGet]
        [Route("{id:Guid}")]
        public async Task<IActionResult> GetCategoryById([FromRoute]Guid id)
        {
            var existingCategory = await categoryRepository.GetById(id);
            if(existingCategory is null)
            {
                return NotFound();
            }

            var response = new CategoryDto
            {
                Id = existingCategory.Id,
                Name = existingCategory.Name,
                UrlHandle = existingCategory.UrlHandle

            };

            return Ok(response);
        }

        //PUT https://localhost:7183/api/categories/{id}
        [HttpPut]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> EditCategory([FromRoute]Guid id, UpdateCategoryRequestDto request)
        {
            //Convert DTO to Domain Model
            var category = new Category
            {
                Id = id,
                Name = request.Name,
                UrlHandle = request.UrlHandle
            };

             category = await categoryRepository.UpdateAsync(category);

            if(category is null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }

        //DELETE https://localhost:7183/api/categories/{id}
        [HttpDelete]
        [Route("{id:Guid}")]
        [Authorize(Roles = "Writer")]
        public async Task<IActionResult> DeleteCategory([FromRoute]Guid id)
        {
              var category =  await categoryRepository.DeleteAsync(id);
            if(category is null)
            {
                return NotFound();
            }

            //Convert Domain Model to DTO
            var response = new CategoryDto
            {
                Id = category.Id,
                Name = category.Name,
                UrlHandle = category.UrlHandle
            };

            return Ok(response);
        }
    }
}
