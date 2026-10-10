using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class RoleController(ILogger<RoleController> logger, IRoleRepository roleRepository) : ControllerBase
    {
        [HttpGet(Name = "GetRoles")]
        public async Task<IEnumerable<RoleDTO>> GetAll()
        {
            var roles = await roleRepository.ReadAsync() ?? [];
            return roles.Select(RoleDTO.ConvertFrom);
        }

        [HttpGet("{id:int}", Name = "GetRoleById")]
        public async Task<ActionResult<RoleDTO>> GetById(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role == null) return NotFound();
            return RoleDTO.ConvertFrom(role);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<Role> roles)
        {
            foreach (var r in roles)
            {
                // OJO: en ProductController esta condición está al revés
                // (ProductId > 0 => Create, lo cual no tiene sentido).
                // Aquí la dejamos correcta: si ya tiene Id, es una edición.
                if (r.RoleId > 0)
                    await roleRepository.UpdateAsync(r);
                else
                    await roleRepository.CreateAsync(r);
            }
            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var role = await roleRepository.FindAsync(id);
            if (role == null) return false;
            return await roleRepository.DeleteAsync(role);
        }
    }
}
