using Microsoft.EntityFrameworkCore;
using ProjectManagement.Application.Common.Interfaces;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Domain.Entities;
using ProjectManagement.Infrastructure.Persistence;

namespace ProjectManagement.Infrastructure.Services;

public class TaskService : ITaskService
{
    private readonly AppDbContext _context;

    public TaskService(AppDbContext context)
    {
        _context = context;
    }

    public async Task<TaskResponseDto> CreateTaskAsync(CreateTaskDto request, Guid userId)
    {
        // Verify the project exists and belongs to the user
        var project = await _context.Projects
            .FirstOrDefaultAsync(p => p.Id == request.ProjectId && p.UserId == userId);

        if (project is null)
        {
            throw new KeyNotFoundException("Project not found or you don't have access to it.");
        }

        var task = new TaskItem
        {
            Id = Guid.NewGuid(),
            Title = request.Title,
            Description = request.Description,
            Status = request.Status,
            Priority = request.Priority,
            DueDate = request.DueDate,
            ProjectId = request.ProjectId
        };

        _context.Tasks.Add(task);
        await _context.SaveChangesAsync();

        return MapToResponseDto(task);
    }

    public async Task<IEnumerable<TaskResponseDto>> GetTasksByProjectAsync(Guid projectId, Guid userId)
    {
        // Verify the project exists and belongs to the user
        var projectExists = await _context.Projects
            .AnyAsync(p => p.Id == projectId && p.UserId == userId);

        if (!projectExists)
        {
            throw new KeyNotFoundException("Project not found or you don't have access to it.");
        }

        var tasks = await _context.Tasks
            .Where(t => t.ProjectId == projectId)
            .OrderByDescending(t => t.Priority)
            .ThenBy(t => t.DueDate)
            .ToListAsync();

        return tasks.Select(MapToResponseDto);
    }

    public async Task<TaskResponseDto> UpdateTaskStatusAsync(Guid taskId, UpdateTaskStatusDto request, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

        if (task is null)
        {
            throw new KeyNotFoundException("Task not found or you don't have access to it.");
        }

        task.Status = request.Status;
        await _context.SaveChangesAsync();

        return MapToResponseDto(task);
    }

    public async Task DeleteTaskAsync(Guid taskId, Guid userId)
    {
        var task = await _context.Tasks
            .FirstOrDefaultAsync(t => t.Id == taskId && t.Project.UserId == userId);

        if (task is null)
        {
            throw new KeyNotFoundException("Task not found or you don't have access to it.");
        }

        _context.Tasks.Remove(task);
        await _context.SaveChangesAsync();
    }

    private static TaskResponseDto MapToResponseDto(TaskItem task)
    {
        return new TaskResponseDto
        {
            Id = task.Id,
            Title = task.Title,
            Description = task.Description,
            Status = task.Status,
            Priority = task.Priority,
            DueDate = task.DueDate,
            ProjectId = task.ProjectId
        };
    }
}