
namespace FitnessApp.Server.Features.Recipes.Models
{
    using System;

    public class DeleteRecipeLogRequestModel
    {
        public int RecipeId { get; set; }
        
        public DateTime DateLogged { get; set; }
    }
}