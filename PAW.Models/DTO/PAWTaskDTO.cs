using System;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;
using PawTask = PAW.Models.Task;

namespace PAW.Models.DTO;

public class PawTaskDTO
{
    [JsonPropertyName("id")]
    public Guid Id { get; set; }
    [JsonPropertyName("taskId")]
    public int TaskId { get; set; }
    [JsonPropertyName("name")]
    [Required(ErrorMessage = "Name is required")]
    [StringLength(255)]
    public string? Name { get; set; }
    [JsonPropertyName("description")]
    [StringLength(1000)]
    public string? Description { get; set; }
    [JsonPropertyName("status")]
    [StringLength(50)]
    public string? Status { get; set; }
    [JsonPropertyName("dueDate")]
    public DateTime? DueDate { get; set; }
    [JsonPropertyName("modifiedBy")]
    [StringLength(255)]
    public string? ModifiedBy { get; set; }
    [JsonPropertyName("comments")]
    public string Comments { get; set; } = string.Empty;
    [JsonPropertyName("createdDate")]
    public DateTime CreatedDate { get; set; }
    [JsonPropertyName("modifiedDate")]
    public DateTime ModifiedDate { get; set; }

    public static PawTaskDTO ConvertFrom(PawTask task)
	{
		return new PawTaskDTO
		{
			Id = Guid.NewGuid(),
			TaskId = task.Id,
			Name = task.Name,
			Description = task.Description,
			Status = task.Status,
			DueDate = task.DueDate,
			ModifiedBy = task.ModifiedBy,
			Comments = string.Empty,
			CreatedDate = task.CreatedAt ?? DateTime.Now,
			ModifiedDate = task.LastModified ?? DateTime.Now
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
