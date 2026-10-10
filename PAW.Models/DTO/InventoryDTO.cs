using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class InventoryDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("inventoryId")]
    public int InventoryId { get; set; }
    [JsonPropertyName("productId")]
    public int? ProductId { get; set; }
    [JsonPropertyName("unitPrice")]
    [Required(ErrorMessage = "The unit price is required.")]
    [Range(0, 99999999.99, ErrorMessage = "The unit price must be between 0 and 99,999,999.99.")]
    public decimal? UnitPrice { get; set; }
    [JsonPropertyName("unitsInStock")]
    [Required(ErrorMessage = "The units in stock are required.")]
    [Range(0, int.MaxValue, ErrorMessage = "The units in stock cannot be negative.")]
    public int? UnitsInStock { get; set; }
    [JsonPropertyName("lastUpdated")]
    public DateTime? LastUpdated { get; set; }
    [JsonPropertyName("dateAdded")]
    public DateTime? DateAdded { get; set; }
    [JsonPropertyName("modifiedBy")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("comments")]
    public string? Comments { get; set; }
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static InventoryDTO ConvertFrom(Inventory inventory)
	{
		return new InventoryDTO
		{
			Id = inventory.Id,
			InventoryId = inventory.InventoryId,
			ProductId = inventory.ProductId,
			UnitPrice = inventory.UnitPrice,
			UnitsInStock = inventory.UnitsInStock,
			LastUpdated = inventory.LastUpdated,
			DateAdded = inventory.DateAdded,
			ModifiedBy = inventory.ModifiedBy,
			Comments = inventory.Comments,
			CreatedDate = inventory.CreatedDate,
			ModifiedDate = inventory.ModifiedDate
		};
	}
    public static Inventory ConvertTo(InventoryDTO dto)
    {
        return new Inventory
        {
            InventoryId = dto.InventoryId,
            ProductId = dto.ProductId,
            UnitPrice = dto.UnitPrice,
            UnitsInStock = dto.UnitsInStock,
            LastUpdated = dto.LastUpdated ?? DateTime.Now,
            DateAdded = dto.DateAdded,
            ModifiedBy = dto.ModifiedBy
        };
    }
}
