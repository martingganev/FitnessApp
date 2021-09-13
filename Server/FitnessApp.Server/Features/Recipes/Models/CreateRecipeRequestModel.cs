namespace FitnessApp.Server.Features.Recipes.Models
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models;

    public class CreateRecipeRequestModel
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
        
        public string NotesAndTips { get; set; }

        [Required]
        public IEnumerable<ProductQuantityModel> Products { get; set; } = new HashSet<ProductQuantityModel>();
    }
}
