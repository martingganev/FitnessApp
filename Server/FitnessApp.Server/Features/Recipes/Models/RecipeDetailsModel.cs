using System;
namespace FitnessApp.Server.Features.Recipes.Models
{
    public class RecipeDetailsModel : RecipeListingModel
    {
        public string Description { get; set; }
        
        public string NotesAndTips { get; set; }
    }
}
