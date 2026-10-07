using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "The name is required.")]
    [StringLength(255)]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("description")]
    [StringLength(1000)]
    public string? Description { get; set; }
    [JsonPropertyName("rating")]
    [Range(0, 5, ErrorMessage = "The rating must be between 0 and 5.")]
    public int Rating { get; set; }
    [JsonPropertyName("inventoryId")]
    public int? InventoryId { get; set; }
    [JsonPropertyName("supplierId")]
    public int? SupplierId { get; set; }
    [JsonPropertyName("categoryId")]
    public int? CategoryId { get; set; }
    [JsonPropertyName("modifiedBy")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    [StringLength(100)]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static ProductDTO ConvertFrom(Product product)
    {
        return new ProductDTO
        {
            Id = Guid.NewGuid(),
            ProductId = product.ProductId,
            Name = product.ProductName ?? string.Empty,
            Description = product.Description,
            Rating = (int)(product.Rating ?? 0),
            InventoryId = product.InventoryId,
            SupplierId = product.SupplierId,
            CategoryId = product.CategoryId,
            ModifiedBy = product.ModifiedBy,
            CreatedBy = product.CreatedBy,
            Comments = string.Empty, // Assuming comments are not present in the Product entity
            CreatedDate = product.LastModified ?? DateTime.Now, // Assuming LastModified is used as CreatedDate
            ModifiedDate = product.LastModified ?? DateTime.Now // Assuming LastModified is used as ModifiedDate
        };
    }

    public static Product ConvertTo(ProductDTO productDTO)
    {
        return new Product
        {
            ProductId = productDTO.ProductId,
            ProductName = productDTO.Name,
            Description = productDTO.Description,
            Rating = productDTO.Rating,
            InventoryId = productDTO.InventoryId,
            SupplierId = productDTO.SupplierId,
            CategoryId = productDTO.CategoryId,
            ModifiedBy = productDTO.ModifiedBy,
            CreatedBy = productDTO.CreatedBy,
            LastModified = DateTime.Now // the table only has LastModified
        };
    }
}
