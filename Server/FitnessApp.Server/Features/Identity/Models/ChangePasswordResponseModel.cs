namespace FitnessApp.Server.Features.Identity.Models
{
    using System.Collections.Generic;

    public class ChangePasswordResponseModel
    {
        public string Token { get; set; }

        public bool IsSuccess { get; set; }

        public IEnumerable<string> Errors { get; set; }
    }
}