
namespace FitnessApp.Server.Data.Models.Training
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models.Base;

    public class UserWorkout : DeletableEntity
    {

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int WorkoutId { get; set; }

        public Workout Workout { get; set; }

        [Required]
        public DateTime DateLogged { get; set; }
    }
}
