using System.Collections.Generic;

namespace FitnessApp.Server.Features.Identity.Models
{
    using FitnessApp.Server.Data.Models.Training;
    using FitnessApp.Server.Data.Models;
    
    public class UpdateUserPersonalInfoRequestModel
    {
        public string Email { get; set; }
        
        public string UserName { get; set; }

        public double Height { get; set; }

        public double Weight { get; set; }

        public TrainingType TrainingGoal { get; set; }
        
        public long DailyCalorieGoal { get; set; }
        
        public Gender Gender { get; set; }

        public IEnumerable<string> WorkoutSchedule { get; set; }
    }
}