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
    public class InventoryController(ILogger<InventoryController> logger, IInventoryRepository inventoryRepository) : ControllerBase
    {
        [HttpGet(Name = "GetInventories")]
        public async Task<IEnumerable<InventoryDTO>> GetAll()
        {
            var items = await inventoryRepository.ReadAsync() ?? [];
            return items.Select(InventoryDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetInventoryById")]
        public async Task<ActionResult<InventoryDTO>> GetById(int id)
        {
            var item = await inventoryRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return InventoryDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<InventoryDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = InventoryDTO.ConvertTo(dto);
                // InventoryId > 0 => already exists, so update; otherwise create
                var saved = await inventoryRepository.UpsertAsync(entity, isUpdating: entity.InventoryId > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await inventoryRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await inventoryRepository.DeleteAsync(item);
        }
    }
}



