using System.ComponentModel.DataAnnotations.Schema;

namespace FitnessApp.Server.Data.Models
{
    using Microsoft.AspNetCore.Identity;
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using Base;
    using Eating;
    using Training;

    public class User : IdentityUser, IEntity
    {
        public double Height { get; set; }

        public double Weight { get; set; }

        [Required]
        public TrainingType TrainingGoal { get; set; }
        
        [Required]
        public long TargetProteins { get; set; }
        
        [Required]
        public long TargetCarbs { get; set; }
        
        [Required]
        public long TargetFats { get; set; }

        [Required]
        public long DailyCalorieGoal { get; set; }
        
        [NotMapped]
        public long WeeklyCalorieGoal => DailyCalorieGoal * 7;

        public Gender Gender { get; set; } = Gender.Male;

        [Required]
        public string WorkoutSchedule { get; set; }

        public DateTime CreatedOn { get; set; }

        public string CreatedBy { get; set; }

        public DateTime? ModifiedOn { get; set; }

        public string ModifiedBy { get; set; }
        
        public IEnumerable<UserRecipe> UsersRecipes { get; set; } = new HashSet<UserRecipe>();

        public IEnumerable<UserWorkout> UsersWorkouts { get; set; } = new HashSet<UserWorkout>();
    }
}
