namespace FitnessApp.Server.Infrastructure.Services
{
    public interface ICurrentUserService
    {
        string GetUserName();

        string GetId();

        bool IsAdmin();
    }
}
