using ProjectManagement.Domain.Enums;

namespace ProjectManagement.Application.DTOs.Tasks;

public class UpdateTaskStatusDto
{
    public Status Status { get; set; }
}
