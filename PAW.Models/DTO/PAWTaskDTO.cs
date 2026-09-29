using System;
using System.Text.Json.Serialization;
namespace PAW.Models.DTO;

public class PAWTaskDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("taskId")]
    public int TaskId { get; set; }
    [JsonPropertyName("name")]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    public string? Description { get; set; }
    [JsonPropertyName("status")]
    public string? Status { get; set; }
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }
    [JsonPropertyName("modifiedBy")]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static PAWTaskDTO ConvertFrom(PAWTask task)
	{
		return new PAWTaskDTO
		{
			Id = task.Id,
			TaskId = task.TaskId,
			Name = task.Name,
			Description = task.Description,
			Status = task.Status,
			DueDate = task.DueDate,
			ModifiedBy = task.ModifiedBy,
			Comments = task.Comments,
			CreatedDate = task.CreatedDate,
			ModifiedDate = task.ModifiedDate
		};
	}
    public static PawTask ConvertTo(PawTaskDTO dto)
    {
        return new PawTask
        {
            Id = dto.TaskId,
            Name = dto.Name,
            Description = dto.Description,
            Status = dto.Status,
            DueDate = dto.DueDate,
            CreatedAt = dto.TaskId > 0 ? dto.CreatedDate : DateTime.Now,
            LastModified = DateTime.Now,
            ModifiedBy = dto.ModifiedBy
        };
    }

}
