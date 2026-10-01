using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class ComponentDTO
{
    [JsonPropertyName("id")]
    public decimal Id { get; set; }
    [JsonPropertyName("name")]
    public string Name { get; set; } = string.Empty;
    [JsonPropertyName("content")]
    public string Content { get; set; } = string.Empty;

    public static ComponentDTO ConvertFrom(Component component)
    {
        return new ComponentDTO
        {
            Id = component.Id,
            Name = component.Name,
            Content = component.Content
        };
    }

    public static Component ConvertTo(ComponentDTO dto)
    {
        return new Component
        {
            Id = dto.Id,
            Name = dto.Name,
            Content = dto.Content
        };
    }
}
