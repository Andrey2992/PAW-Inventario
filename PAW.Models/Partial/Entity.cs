using System.ComponentModel.DataAnnotations.Schema;
using System.Runtime.Serialization;

namespace PAW.Models;

public interface IEntity
{
    [NotMapped]
    Guid Id { get; set; }
    [NotMapped]
    string Comments { get; set; }
    [NotMapped]
    DateTime CreatedDate { get; set; }
    [NotMapped]
    DateTime ModifiedDate { get; set; }
}

public class Entity : IEntity
{
    [NotMapped]
    public Guid Id { get; set; }
    [NotMapped]
    public string Comments { get; set; }
    [NotMapped]
    public DateTime CreatedDate { get; set; }
    [NotMapped]
    public DateTime ModifiedDate { get; set; }
}

