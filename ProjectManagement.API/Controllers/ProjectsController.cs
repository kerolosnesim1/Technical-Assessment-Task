using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ProjectManagement.Application.Common;
using ProjectManagement.Application.Common.Interfaces;
using ProjectManagement.Application.DTOs.Projects;
using ProjectManagement.Application.Validators.Projects;
using System.Security.Claims;

namespace ProjectManagement.API.Controllers;

[ApiController]
[Route("api/[controller]")]
[Authorize]
public class ProjectsController : ControllerBase
{
    private readonly IProjectService _projectService;

    public ProjectsController(IProjectService projectService)
    {
        _projectService = projectService;
    }

    private Guid GetUserId()
    {
        var userIdClaim = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
        return Guid.Parse(userIdClaim!);
    }

    [HttpPost]
    public async Task<IActionResult> CreateProject([FromBody] CreateProjectDto request)
    {
        var validator = new CreateProjectDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _projectService.CreateProjectAsync(request, GetUserId());
        return Ok(ApiResponse<ProjectResponseDto>.SuccessResponse(result, "Project created successfully"));
    }

    [HttpGet]
    public async Task<IActionResult> GetAllProjects()
    {
        var result = await _projectService.GetAllProjectsAsync(GetUserId());
        return Ok(ApiResponse<IEnumerable<ProjectResponseDto>>.SuccessResponse(result, "Projects retrieved successfully"));
    }

    [HttpGet("{id:guid}")]
    public async Task<IActionResult> GetProjectById(Guid id)
    {
        var result = await _projectService.GetProjectByIdAsync(id, GetUserId());
        return Ok(ApiResponse<ProjectResponseDto>.SuccessResponse(result, "Project retrieved successfully"));
    }

    [HttpPut("{id:guid}")]
    public async Task<IActionResult> UpdateProject(Guid id, [FromBody] UpdateProjectDto request)
    {
        var validator = new UpdateProjectDtoValidator();
        var validationResult = validator.Validate(request);

        if (!validationResult.IsValid)
        {
            return BadRequest(ApiResponse.FailureResponse(
                validationResult.Errors.Select(e => e.ErrorMessage).FirstOrDefault() ?? "Validation failed"));
        }

        var result = await _projectService.UpdateProjectAsync(id, request, GetUserId());
        return Ok(ApiResponse<ProjectResponseDto>.SuccessResponse(result, "Project updated successfully"));
    }

    [HttpDelete("{id:guid}")]
    public async Task<IActionResult> DeleteProject(Guid id)
    {
        await _projectService.DeleteProjectAsync(id, GetUserId());
        return Ok(ApiResponse.SuccessResponse("Project deleted successfully"));
    }
}