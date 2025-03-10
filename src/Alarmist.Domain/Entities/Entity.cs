using System.ComponentModel.DataAnnotations;

namespace Alarmist.Domain.Entities;

public class Entity
{
    [Key]
    public Guid Id { get; private set; }
    public DateTime CreatedAt { get; private set; }
    public DateTime UpdatedAt { get; private set; }

    public bool IsDeleted { get; private set; } = false;
}