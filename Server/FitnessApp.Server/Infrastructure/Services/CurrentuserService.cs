namespace FitnessApp.Server.Infrastructure.Services
{
    using Microsoft.AspNetCore.Http;
    using System.Security.Claims;
    using FitnessApp.Server.Infrastructure.Extensions;

    public class CurrentUserService : ICurrentUserService
    {
        private readonly ClaimsPrincipal user;

        public CurrentUserService(IHttpContextAccessor httpContextAccessor)
            => this.user = httpContextAccessor.HttpContext?.User;

        public string GetUserName()
            => this.user?.Identity?.Name;

        public string GetId()
            => this.user?.GetId();

        public bool IsAdmin()
            => this.user == null ? false : this.user.IsInRole("Admin");
    }
}
