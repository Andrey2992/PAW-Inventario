using PAW.Models.DTO;

namespace PAW.Web.Models
{
    public class ComponentDetailsViewModel
    {
        // Component no tiene relación con inventario, categoría ni supplier,
        // por eso el modelo solo contiene el componente.
        public ComponentDTO Component { get; set; } = new();
    }
}
