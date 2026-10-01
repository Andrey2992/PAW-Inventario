using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ComponentController(ILogger<ComponentController> logger, IComponentRepository componentRepository) : ControllerBase
    {
        [HttpGet(Name = "GetComponents")]
        public async Task<IEnumerable<ComponentDTO>> GetAll()
        {
            var items = await componentRepository.ReadAsync() ?? [];
            return items.Select(ComponentDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetComponentById")]
        public async Task<ActionResult<ComponentDTO>> GetById(int id)
        {
            var item = await componentRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return ComponentDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<ComponentDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = ComponentDTO.ConvertTo(dto);
                // Id > 0 => already exists, so update; otherwise create
                var saved = await componentRepository.UpsertAsync(entity, isUpdating: entity.Id > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await componentRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await componentRepository.DeleteAsync(item);
        }
    }
}
