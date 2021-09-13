using System;

namespace FitnessApp.Server.Features.Identity.Models
{
    public class DashboardRecipeInfo
    {
        public int RecipeId { get; set; }
        
        public DateTime DateLogged { get; set; }
        
        public string Name { get; set; }

        public double Calories { get; set; }

        public double Carbs { get; set; }
        
        public double Fats { get; set; }
        
        public double Protein { get; set; }
    }
}