using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class CategoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("categoryId")]
    public int CategoryId { get; set; }
    [JsonPropertyName("categoryName")]
    public string? CategoryName { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static CategoryDTO ConvertFrom(Category category)
    {
        return new CategoryDTO
        {
            Id = Guid.NewGuid(),
            CategoryId = category.CategoryId,
            CategoryName = category.CategoryName,
            Description = category.Description,
            ModifiedBy = category.ModifiedBy,
            Comments = string.Empty,
            CreatedDate = category.LastModified ?? DateTime.Now, // the table only has LastModified
            ModifiedDate = category.LastModified ?? DateTime.Now
        };
    }

    public static Category ConvertTo(CategoryDTO dto)
    {
        return new Category
        {
            CategoryId = dto.CategoryId,
            CategoryName = dto.CategoryName,
            Description = dto.Description,
            ModifiedBy = dto.ModifiedBy,
            LastModified = DateTime.Now
        };
    }
}
