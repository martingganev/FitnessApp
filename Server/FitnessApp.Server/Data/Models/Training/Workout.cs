namespace FitnessApp.Server.Data.Models.Training
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models.Base;

    public class Workout : DeletableEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public int TimeToFinish { get; set; }
        
        [Required]
        public Difficulty Difficulty { get; set; }

        [Required]
        public string Photo { get; set; }

        public string Description { get; set; }

        public IEnumerable<WorkoutExercise> WorkoutsExcercises { get; set; } = new HashSet<WorkoutExercise>();

        public IEnumerable<UserWorkout> UsersWorkouts { get; set; } = new HashSet<UserWorkout>();

        [Required]
        public double CaloriesBurned { get; set; }

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
