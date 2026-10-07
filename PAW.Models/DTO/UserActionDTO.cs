using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class UserActionDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("userActionId")]
    public int UserActionId { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(50)]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    [StringLength(100)]
    public string? Description { get; set; }

    public static UserActionDTO ConvertFrom(UserAction userAction)
    {
        return new UserActionDTO
        {
            Id = Guid.NewGuid(),
            UserActionId = userAction.Id,
            Name = userAction.Name,
            Description = userAction.Description
        };
    }

    public static UserAction ConvertTo(UserActionDTO userActionDTO)
    {
        return new UserAction
        {
            Id = userActionDTO.UserActionId,
            Name = userActionDTO.Name,
            Description = userActionDTO.Description
        };
    }
}
