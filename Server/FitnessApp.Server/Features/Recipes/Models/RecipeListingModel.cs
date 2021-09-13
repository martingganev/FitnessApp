namespace FitnessApp.Server.Features.Recipes.Models
{
    using System.Collections.Generic;
    using FitnessApp.Server.Data.Models;

    public class RecipeListingModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public int TimeToFinish { get; set; }
        
        public double TotalCalories { get; set; }

        public string Difficulty { get; set; }

        public string Photo { get; set; }
        
        public bool IsMine { get; set; }

        public bool IsAdmin { get; set; }

        public IEnumerable<ProductQuantityListingModel> Products { get; set; } = new HashSet<ProductQuantityListingModel>();
    }
}
