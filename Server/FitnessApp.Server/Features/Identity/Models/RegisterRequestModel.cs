namespace FitnessApp.Server.Features.Identity.Models
{
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Data.Models.Training;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;

    public class RegisterRequestModel
    {
        [Required]
        public string UserName { get; set; }

        [Required]
        [EmailAddress]
        public string Email { get; set; }

        [Required]
        public string Password { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public Gender Gender { get; set; }

        [Required]
        public TrainingType TrainingGoal { get; set; }

        [Required]
        public long DailyCalorieGoal { get; set; }

        [Required]
        public IEnumerable<string> WorkoutSchedule { get; set; }
    }
}
