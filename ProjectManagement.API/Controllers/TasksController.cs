using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Common.Interfaces;
using ProjectManagement.Application.DTOs.Tasks;
using ProjectManagement.Application.Validators.Tasks;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class TasksController : ControllerBase
{
    private readonly ITaskService _taskService;

    public TasksController(ITaskService taskService)
    {
        _taskService = taskService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost]
    public async Task<IActionResult> CreateTask([FromBody] CreateTaskDto request)
    {
        var validator = new CreateTaskDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _taskService.CreateTaskAsync(request, GetUserId());
        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(result, "Task created successfully"));
    }

    [HttpGet("project/{projectId:guid}")]
    public async Task<IActionResult> GetTasksByProject(Guid projectId)
    {
        var result = await _taskService.GetTasksByProjectAsync(projectId, GetUserId());
        return Ok(ApiResponse<IEnumerable<TaskResponseDto>>.SuccessResponse(result, "Tasks retrieved successfully"));
    }

    [HttpPut("{taskId:guid}/status")]
    public async Task<IActionResult> UpdateTaskStatus(Guid taskId, [FromBody] UpdateTaskStatusDto request)
    {
        var validator = new UpdateTaskStatusDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _taskService.UpdateTaskStatusAsync(taskId, request, GetUserId());
        return Ok(ApiResponse<TaskResponseDto>.SuccessResponse(result, "Task status updated successfully"));
    }

    [HttpDelete("{taskId:guid}")]
    public async Task<IActionResult> DeleteTask(Guid taskId)
    {
        await _taskService.DeleteTaskAsync(taskId, GetUserId());
        return Ok(ApiResponse.SuccessResponse("Task deleted successfully"));
    }
}