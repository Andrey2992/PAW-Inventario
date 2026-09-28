using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json.Serialization;

namespace PAW.Models.DTO;
public class UserRoleDTO
{
    [JsonPropertyName("id")]
    public int Id { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("userName")]
    public string? UserName { get; set; }
    [JsonPropertyName("roleId")]
    public int RoleId { get; set; }
    [JsonPropertyName("roleName")]
    public string? RoleName { get; set; } 
    public static UserRoleDTO ConvertFrom(UserRole userRole, IEnumerable<User>? users = null, IEnumerable<Role>? roles = null)
    {
        var userId = userRole.UserId ?? 0;
        var roleId = userRole.RoldId ?? 0;
        
        return new UserRoleDTO
        {
            Id = userRole.Id,
            UserId = userId,
            UserName = users?.FirstOrDefault(u => u.UserId == userId)?.Username,
            RoleId = roleId,
            RoleName = roles?.FirstOrDefault(r => r.RoleId == roleId)?.RoleName
        };
    }
    public static UserRole ConvertTo(UserRoleDTO dto)
    {
        return new UserRole
        {
            Id = dto.Id,
            UserId = dto.UserId,
            RoldId = dto.RoleId
        };
    }
}