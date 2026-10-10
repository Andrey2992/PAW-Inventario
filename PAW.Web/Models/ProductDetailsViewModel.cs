using PAW.Models.DTO;

namespace PAW.Web.Models;

public class ProductDetailsViewModel
{
    public ProductDTO Product { get; set; } = new();
    public InventoryDTO? Inventory { get; set; }
    public CategoryDTO? Category { get; set; }
    public SupplierDTO? Supplier { get; set; }

    internal static List<ProductDetailsViewModel> BuildList(List<ProductDTO> products, IEnumerable<InventoryDTO> inventories, IEnumerable<CategoryDTO> categories, IEnumerable<SupplierDTO> suppliers)
    {
        throw new NotImplementedException();
    }
}
