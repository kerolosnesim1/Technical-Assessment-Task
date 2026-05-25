using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Common.Interfaces;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Persistence;

namespace ProjectManagement.Infrastructure.Services;

public class ProjectService : IProjectService
{
    private readonly AppDbContext _context;

    public ProjectService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<ProjectResponseDto> CreateProjectAsync(CreateProjectDto request, Guid userId)
    {
        var project = new Project
        {
            Id = Guid.NewGuid(),
            Name = request.Name,
            Description = request.Description,
            CreatedAt = DateTime.UtcNow,
            UserId = userId
        };

        _context.Projects.Add(project);
        await _context.SaveChangesAsync();

        return MapToResponseDto(project);
    }

    public async Task<IEnumerable<ProjectResponseDto>> GetAllProjectsAsync(Guid userId)
    {
        var projects = await _context.Projects
            .Where(p => p.UserId == userId)
            .OrderByDescending(p => p.CreatedAt)
            .ToListAsync();

        return projects.Select(MapToResponseDto);
    }

    public async Task<ProjectResponseDto> GetProjectByIdAsync(Guid projectId, Guid userId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found or you don't have access to it.");
        }

        return MapToResponseDto(project);
    }

    public async Task<ProjectResponseDto> UpdateProjectAsync(Guid projectId, UpdateProjectDto request, Guid userId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found or you don't have access to it.");
        }

        project.Name = request.Name;
        project.Description = request.Description;

        await _context.SaveChangesAsync();

        return MapToResponseDto(project);
    }

    public async Task DeleteProjectAsync(Guid projectId, Guid userId)
    {
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == projectId && p.UserId == userId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found or you don't have access to it.");
        }

        _context.Projects.Remove(project);
        await _context.SaveChangesAsync();
    }

    private static ProjectResponseDto MapToResponseDto(Project project)
    {
        return new ProjectResponseDto
        {
            Id = project.Id,
            Name = project.Name,
            Description = project.Description,
            CreatedAt = project.CreatedAt,
            UserId = project.UserId
        };
    }
}