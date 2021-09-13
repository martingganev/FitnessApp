namespace FitnessApp.Server.Features.Products.Models
{
    public class UpdateProductRequestModel
    {
        public string Name { get; set; }

        public double Carbs { get; set; }

        public double Fats { get; set; }

        public double Protein { get; set; }

        public double Sodium { get; set; }

        public double Sugar { get; set; }
    }
}
