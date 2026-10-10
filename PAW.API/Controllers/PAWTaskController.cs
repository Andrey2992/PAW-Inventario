using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;


namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PawTaskController(ILogger<PawTaskController> logger, IPawTaskRepository pawTaskRepository) : ControllerBase
    {
        [HttpGet(Name = "GetPawTasks")]
        public async Task<IEnumerable<PawTaskDTO>> GetAll()
        {
            var items = await pawTaskRepository.ReadAsync() ?? [];
            return items.Select(PawTaskDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetPawTaskById")]
        public async Task<ActionResult<PawTaskDTO>> GetById(int id)
        {
            var item = await pawTaskRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return PawTaskDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<PawTaskDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = PawTaskDTO.ConvertTo(dto);
                // Id > 0 => already exists, so update; otherwise create
                var saved = await pawTaskRepository.UpsertAsync(entity, isUpdating: entity.Id > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await pawTaskRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await pawTaskRepository.DeleteAsync(item);
        }
    }
}

