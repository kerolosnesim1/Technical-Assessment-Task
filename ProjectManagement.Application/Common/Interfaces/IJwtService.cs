using ProjectManagement.Domain.Entities;

namespace ProjectManagement.Application.Common.Interfaces;

public interface IJwtService
{
    string GenerateToken(User user);
}