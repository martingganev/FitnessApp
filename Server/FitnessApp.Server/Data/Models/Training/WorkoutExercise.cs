namespace FitnessApp.Server.Data.Models.Training
{
    using System.ComponentModel.DataAnnotations;

    public class WorkoutExercise
    {
        [Required]
        public int WorkoutId { get; set; }

        public Workout Workout { get; set; }

        [Required]
        public int ExerciseId { get; set; }

        public Exercise Exercise { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }
    }
}
