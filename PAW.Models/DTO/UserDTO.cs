using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;
public class UserDTO
{
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("username")]
    public string? Username { get; set; }
    [JsonPropertyName("email")]
    public string? Email { get; set; }
    [JsonPropertyName("passwordHash")]
    public string? PasswordHash { get; set; }
    [JsonPropertyName("isActive")]
    public bool IsActive { get; set; }
    [JsonPropertyName("roleId")]
    public int? RoleId { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }
    public static UserDTO ConvertFrom(User user)
    {
        return new UserDTO
        {
            UserId = user.UserId,
            Username = user.Username,
            Email = user.Email,
            PasswordHash = user.PasswordHash,
            IsActive = user.IsActive ?? true,
            RoleId = user.RoleId,
            CreatedAt = user.CreatedAt
        };
    }
    public static User ConvertTo(UserDTO dto)
    {
        return new User
        {
            UserId = dto.UserId,
            Username = dto.Username,
            Email = dto.Email,
            PasswordHash = dto.PasswordHash,
            IsActive = dto.IsActive,
            RoleId = dto.RoleId,
            CreatedAt = dto.CreatedAt ?? DateTime.Now
        };
    }
}

