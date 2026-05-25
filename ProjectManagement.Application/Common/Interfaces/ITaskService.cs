using ProjectManagement.Application.DTOs.Tasks;

namespace ProjectManagement.Application.Common.Interfaces;

public interface ITaskService
{
    Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto request, Guid userId);
    Task<IEnumerable<TaskResponseDto>> GetTasksByProjectAsync(Guid projectId, Guid userId);
    Task<TaskResponseDto> UpdateTaskStatusAsync(Guid taskId, UpdateTaskStatusDto request, Guid userId);
    Task DeleteTaskAsync(Guid taskId, Guid userId);
}