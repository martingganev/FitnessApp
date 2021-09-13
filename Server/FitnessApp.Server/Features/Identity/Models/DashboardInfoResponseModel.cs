namespace FitnessApp.Server.Features.Identity.Models
{
    using System.Collections.Generic;

    public class DashboardInfoResponseModel
    {
        public long TargetCalories { get; set; }
        
        public long CurrentCalories { get; set; }
        
        public double CurrentProteins { get; set; }
        
        public double TargetProteins { get; set; }
        
        public double CurrentCarbs { get; set; }
        
        public double TargetCarbs { get; set; }
        
        public double CurrentFats { get; set; }
        
        public double TargetFats { get; set; }
        
        public double CurrentSugars { get; set; }
        
        public double CurrentSodium { get; set; }

        public double BurnedCalories { get; set; }

        public long TimeSpentTraining { get; set; }

        public bool IsRestDay { get; set; }
        
        public bool IsSuccess { get; set; }
        
        public string Error { get; set; }

        public IEnumerable<DashboardRecipeInfo> LoggedRecipes { get; set; }
        
        public IEnumerable<DashboardWorkoutInfo> LoggedWorkouts { get; set; }
    }
}