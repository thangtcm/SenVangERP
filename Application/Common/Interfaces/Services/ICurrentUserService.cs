using Domain.Abstract;
using System.Security.Claims;

namespace Application.Common.Interfaces.Services
{
    public interface ICurrentUserService : IScopedService
    {
        string? Name { get; }

        string GetUserId();

        string GetUserName();

        string? GetUserEmail();

        bool IsAuthenticated();

        bool IsInRole(string role);

        IEnumerable<Claim>? GetUserClaims();
    }
}
