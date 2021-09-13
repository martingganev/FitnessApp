namespace FitnessApp.Server.Data.Models.Eating
{
    using System.ComponentModel.DataAnnotations;
    using FitnessApp.Server.Data.Models.Base;

    public class Product : DeletableEntity
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        [Required]
        public double Carbs { get; set; }
        public double Sugar { get; set; }

        [Required]
        public double Fats { get; set; }

        [Required]
        public double Protein { get; set; }

        public double Sodium { get; set; }


        public long Calories => (long)(4 * Carbs + 4 * Protein + 9 * Fats);
    }
}
