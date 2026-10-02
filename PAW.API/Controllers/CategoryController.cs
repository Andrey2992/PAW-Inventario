using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CategoryController(ILogger<CategoryController> logger, ICategoryRepository categoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetCategories")]
        public async Task<IEnumerable<CategoryDTO>> GetAll()
        {
            var items = await categoryRepository.ReadAsync() ?? [];
            return items.Select(CategoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetCategoryById")]
        public async Task<ActionResult<CategoryDTO>> GetById(int id)
        {
            var item = await categoryRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return CategoryDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<CategoryDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = CategoryDTO.ConvertTo(dto);
                // CategoryId > 0 => already exists, so update; otherwise create
                var saved = await categoryRepository.UpsertAsync(entity, isUpdating: entity.CategoryId > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await categoryRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await categoryRepository.DeleteAsync(item);
        }
    }
}
