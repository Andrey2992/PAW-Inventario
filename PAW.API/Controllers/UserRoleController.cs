using Microsoft.AspNetCore.Mvc;
using PAW.DataAccess.Repositories;
using PAW.Models;
using PAW.Models.DTO;

namespace PAW.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UserRoleController(
        ILogger<UserRoleController> logger,
        IUserRoleRepository userRoleRepository,
        IUserRepository userRepository,
        IRoleRepository roleRepository) : ControllerBase
    {
        // Aquí sí necesitamos 3 repositorios: UserRole no tiene navegación a
        // User/Role (la BD no tiene esa Foreign Key), así que "traducimos"
        // los IDs a nombres a mano, cargando Users y Roles también.
        [HttpGet(Name = "GetUserRoles")]
        public async Task<IEnumerable<UserRoleDTO>> GetAll()
        {
            var userRoles = await userRoleRepository.ReadAsync() ?? [];
            var users = await userRepository.ReadAsync() ?? [];
            var roles = await roleRepository.ReadAsync() ?? [];

            return userRoles.Select(ur => UserRoleDTO.ConvertFrom(ur, users, roles));
        }

        [HttpGet("{id:int}", Name = "GetUserRoleById")]
        public async Task<ActionResult<UserRoleDTO>> GetById(int id)
        {
            var userRole = await userRoleRepository.FindByIdAsync(id);
            if (userRole == null) return NotFound();

            var users = await userRepository.ReadAsync() ?? [];
            var roles = await roleRepository.ReadAsync() ?? [];
            return UserRoleDTO.ConvertFrom(userRole, users, roles);
        }

        [HttpPost]
        public async Task<bool> Save([FromBody] IEnumerable<UserRole> userRoles)
        {
            foreach (var ur in userRoles)
            {
                if (ur.Id > 0)
                    await userRoleRepository.UpdateAsync(ur);
                else
                    await userRoleRepository.CreateAsync(ur);
            }
            return true;
        }

        [HttpDelete("{id:int}")]
        public async Task<bool> Delete(int id)
        {
            var userRole = await userRoleRepository.FindByIdAsync(id);
            if (userRole == null) return false;
            return await userRoleRepository.DeleteAsync(userRole);
        }
    }
}
