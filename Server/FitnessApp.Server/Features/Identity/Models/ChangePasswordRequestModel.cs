namespace FitnessApp.Server.Features.Identity.Models
{
    public class ChangePasswordRequestModel
    {
        public string OldPassword { get; set; }
        
        public string NewPassword { get; set; }
    }
}