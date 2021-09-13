namespace FitnessApp.Server.Features.Identity
{
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Identity.Models;
    using Microsoft.AspNetCore.Identity;

    public interface IIdentityService
    {
        Task<IdentityResult> Register(RegisterRequestModel model);

        Task<UpdateUserPersonalInfoResponseModel> UpdateUserPersonalInfo(UpdateUserPersonalInfoRequestModel model, string userId);
        
        Task<ChangePasswordResponseModel> ChangeUserPassword(ChangePasswordRequestModel model, string userId);
        
        Task<DashboardInfoResponseModel> GetUserDashboardInfo(string userId);

        Task<GetUserPersonalInfoResponseModel> GetUserPersonalInfo(string userId);
        
        string GenerateJwtToken(string userId, string userName, string role, string secret);
    }
}
