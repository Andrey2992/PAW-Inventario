using PAW.Models.DTO;

namespace PAW.Web.Models
{
    public class CategoryDetailsViewModel
    {
        public CategoryDTO Category { get; set; } = new();

        // Productos de la categoría, cada uno con su inventario, categoría y supplier.
        public List<ProductDetailsViewModel> Products { get; set; } = new();

    }
}
