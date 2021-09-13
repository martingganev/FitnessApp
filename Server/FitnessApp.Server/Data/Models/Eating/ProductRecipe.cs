namespace FitnessApp.Server.Data.Models.Eating
{
    using System.ComponentModel.DataAnnotations;

    public class ProductRecipe
    {
        [Required]
        public int ProductId { get; set; }

        public Product Product { get; set; }

        [Required]
        public int RecipeId { get; set; }

        public Recipe Recipe { get; set; }

        public int Quantity { get; set; }
    }
}
