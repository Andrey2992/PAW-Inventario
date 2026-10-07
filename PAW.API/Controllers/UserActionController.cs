using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserActionController(ILogger<UserActionController> logger, IUserActionRepository userActionRepository) : ControllerBase
    {
        [HttpGet(Name = "GetUserActions")]
        public async Task<IEnumerable<UserActionDTO>> GetAll()
        {
            var items = await userActionRepository.ReadAsync() ?? [];
            return items.Select(UserActionDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetUserActionById")]
        public async Task<ActionResult<UserActionDTO>> GetById(int id)
        {
            var item = await userActionRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return UserActionDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserActionDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = UserActionDTO.ConvertTo(dto);
                // Id > 0 => ya existe, se actualiza; si no, se crea
                var saved = await userActionRepository.UpsertAsync(entity, isUpdating: entity.Id > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await userActionRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await userActionRepository.DeleteAsync(item);
        }
    }
}
