
namespace FitnessApp.Server.Features.Workouts.Models
{
    using System;

    public class DeleteWorkoutLogRequestModel
    {
        public int WorkoutId { get; set; }
        
        public DateTime DateLogged { get; set; }
    }
}