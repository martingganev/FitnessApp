using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using FitnessApp.Server.Data.Models;

namespace FitnessApp.Server.Features.Workouts.Models
{
    public class CreateWorkoutRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public int TimeToFinish { get; set; }

        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        public string Photo { get; set; }

        public string Description { get; set; }

        public IEnumerable<ExercisesSetsRepetitionsModel> Exercises { get; set; } = new HashSet<ExercisesSetsRepetitionsModel>();

        [Required]
        public double CaloriesBurned { get; set; }
    }
}