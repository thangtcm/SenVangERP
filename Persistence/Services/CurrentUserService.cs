using System.Security.Claims;
using Application.Bases;
using Application.Common.Interfaces.Services;
using AutoMapper;

namespace Persistence.Services
{
    public class CurrentUserService() : BaseResponseHandler(), ICurrentUserService
    {
        public string? Name => null;

        public IEnumerable<Claim>? GetUserClaims()
        {
            return [];
        }

        public string? GetUserEmail()
        {
            return null;
        }

        public string GetUserId()
        {
            return "";
        }

        public string GetUserName()
        {
            return "";
        }

        public bool IsAuthenticated()
        {
            return false;
        }

        public bool IsInRole(string role)
        {
            return true;
        }
    }
}
