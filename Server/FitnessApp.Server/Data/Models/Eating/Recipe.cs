namespace FitnessApp.Server.Data.Models.Eating
{
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models.Base;

    public class Recipe : DeletableEntity
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
        
        
        public string NotesAndTips { get; set; }

        public IEnumerable<ProductRecipe> ProductsQuantities { get; set; } = new HashSet<ProductRecipe>();

        public IEnumerable<UserRecipe> UsersRecipes { get; set; } = new HashSet<UserRecipe>();

        [Required]
        public string UserId { get; set; }

        public User User { get; set; }
    }
}
