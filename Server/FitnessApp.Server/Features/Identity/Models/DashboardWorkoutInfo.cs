namespace FitnessApp.Server.Features.Identity.Models
{
    using System;

    public class DashboardWorkoutInfo
    {
        public int WorkoutId { get; set; }
        
        public DateTime DateLogged { get; set; }
        
        public string Name { get; set; }

        public double CaloriesBurned { get; set; }

        public int TimeTrained { get; set; }
    }
}