namespace FitnessApp.Server.Features.Products.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateProductRequestModel
    {
        [Required]
        public string Name { get; set; }

        [Required]
        public double Carbs { get; set; }

        [Required]
        public double Fats { get; set; }

        [Required]
        public double Protein { get; set; }

        public double Sodium { get; set; }

        public double Sugar { get; set; }
    }
}
