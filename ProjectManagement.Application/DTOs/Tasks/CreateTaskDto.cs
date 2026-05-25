using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.DTOs.Tasks;

public class CreateTaskDto
{
    public string Title { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public Status Status { get; set; } = Status.Todo;
    public TaskPriority Priority { get; set; } = TaskPriority.medium;
    public DateTime DueDate { get; set; }
    public Guid ProjectId { get; set; }
}
