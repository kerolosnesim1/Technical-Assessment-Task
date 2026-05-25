using ProjectManagement.Application.DTOs.Projects;

namespace ProjectManagement.Application.Common.Interfaces;

public interface IProjectService
{
    Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto request, Guid userId);
    Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync(Guid userId);
    Task<ProjectResponseDto> GetProjectByIdAsync(Guid projectId, Guid userId);
    Task<ProjectResponseDto> UpdateProjectAsync(Guid projectId, UpdateProjectDto request, Guid userId);
    Task DeleteProjectAsync(Guid projectId, Guid userId);
}