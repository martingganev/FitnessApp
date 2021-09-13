namespace FitnessApp.Server.Data.Models.Training
{
    using System.ComponentModel.DataAnnotations;

    public class Exercise
    {
        [Required]
        [Key]
        public int Id { get; set; }

        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
