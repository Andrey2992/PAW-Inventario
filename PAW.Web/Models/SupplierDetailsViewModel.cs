using PAW.Models.DTO;

namespace PAW.Web.Models
{
    public class SupplierDetailsViewModel
    {
        public SupplierDTO Supplier { get; set; } = new();

        // Productos del supplier, cada uno con su inventario, categoría y supplier.
        public List<ProductDetailsViewModel> Products { get; set; } = new();
    }
}
