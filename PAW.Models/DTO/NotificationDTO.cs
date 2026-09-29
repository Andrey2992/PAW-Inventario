using System.Text.Json.Serialization;

namespace PAW.Models.DTO;

public class NotificationDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("notificationId")]
    public int NotificationId { get; set; }
    [JsonPropertyName("userId")]
    public int UserId { get; set; }
    [JsonPropertyName("message")]
    public string Message { get; set; } = string.Empty;
    [JsonPropertyName("isRead")]
    public bool IsRead { get; set; }
    [JsonPropertyName("createdAt")]
    public DateTime? CreatedAt { get; set; }

    public static NotificationDTO ConvertFrom(Notification notification)
    {
        return new NotificationDTO
        {
            Id = Guid.NewGuid(),
            NotificationId = notification.Id,
            UserId = notification.UserId,
            Message = notification.Message,
            IsRead = notification.IsRead ?? false,
            CreatedAt = notification.CreatedAt
        };
    }

    public static Notification ConvertTo(NotificationDTO notificationDTO)
    {
        return new Notification
        {
            Id = notificationDTO.NotificationId,
            UserId = notificationDTO.UserId,
            Message = notificationDTO.Message,
            IsRead = notificationDTO.IsRead,
            CreatedAt = notificationDTO.CreatedAt
        };
    }
}
