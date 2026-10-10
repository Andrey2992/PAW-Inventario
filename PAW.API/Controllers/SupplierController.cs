using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class SupplierController(ILogger<SupplierController> logger, ISupplierRepository supplierRepository) : ControllerBase
    {
        [HttpGet(Name = "GetSuppliers")]
        public async Task<IEnumerable<SupplierDTO>> GetAll()
        {
            var items = await supplierRepository.ReadAsync() ?? [];
            return items.Select(SupplierDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetSupplierById")]
        public async Task<ActionResult<SupplierDTO>> GetById(int id)
        {
            var item = await supplierRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return SupplierDTO.ConvertFrom(item);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<SupplierDTO> items)
        {
            var allSaved = true;
            foreach (var dto in items)
            {
                var entity = SupplierDTO.ConvertTo(dto);
                // SupplierId > 0 => already exists, so update; otherwise create
                var saved = await supplierRepository.UpsertAsync(entity, isUpdating: entity.SupplierId > 0);
                allSaved &= saved;
            }
            return allSaved;
        }

        [HttpDelete("{id:int}")]
        public async Task<ActionResult<bool>> Delete(int id)
        {
            var item = await supplierRepository.FindAsync(id);
            if (item is null)
                return NotFound();

            return await supplierRepository.DeleteAsync(item);
        }
    }
}
