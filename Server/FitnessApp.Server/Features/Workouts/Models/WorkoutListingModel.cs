using System.Collections.Generic;
using FitnessApp.Server.Data.Models;

namespace FitnessApp.Server.Features.Workouts.Models
{
    public class WorkoutListingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TimeToFinish { get; set; }

        public string Difficulty { get; set; }

        public string Photo { get; set; }

        public string Description { get; set; }

        public IEnumerable<ExerciseInfoModel> Exercises { get; set; } = new HashSet<ExerciseInfoModel>();

        public double CaloriesBurned { get; set; }

        public string Username { get; set; }

        public bool IsMine { get; set; }

        public bool IsAdmin { get; set; }
    }
}