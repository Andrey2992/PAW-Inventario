using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ProductDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("productId")]
    public int ProductId { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; }
    [JsonPropertyName("description")]
    public string Description { get; set; }
    [JsonPropertyName("rating")]
    public int Rating { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("createdBy")]
    public string? CreatedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; }
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
            Name = product.ProductName!,
            Description = product.Description!,
            Rating = (int)(product.Rating ?? 0),
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
                ModifiedBy = productDTO.ModifiedBy,
                CreatedBy = productDTO.CreatedBy,
                LastModified = productDTO.ModifiedDate
            };
    }
}
