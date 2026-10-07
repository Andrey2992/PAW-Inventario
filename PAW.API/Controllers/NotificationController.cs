using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class NotificationController(ILogger<NotificationController> logger, INotificationRepository notificationRepository) : ControllerBase
    {
        [HttpGet(Name = "GetNotifications")]
        public async Task<IEnumerable<NotificationDTO>> GetAll()
        {
            var items = await notificationRepository.ReadAsync() ?? [];
            return items.Select(NotificationDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetNotificationById")]
        public async Task<ActionResult<NotificationDTO>> GetById(int id)
        {
            var item = await notificationRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return NotificationDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<NotificationDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = NotificationDTO.ConvertTo(dto);
                // Id > 0 => ya existe, se actualiza; si no, se crea
                var saved = await notificationRepository.UpsertAsync(entity, isUpdating: entity.Id > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await notificationRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await notificationRepository.DeleteAsync(item);
        }
    }
}
