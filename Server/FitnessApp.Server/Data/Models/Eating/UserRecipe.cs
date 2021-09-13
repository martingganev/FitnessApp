using FitnessApp.Server.Data.Models.Base;

namespace FitnessApp.Server.Data.Models.Eating
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models.Eating;

    public class UserRecipe : DeletableEntity
    {
        [Required]
        public string UserId { get; set; }

        public User User { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        [Required]
        public DateTime DateLogged { get; set; }
    }
}
