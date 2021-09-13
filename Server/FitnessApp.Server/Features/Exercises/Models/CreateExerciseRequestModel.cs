namespace FitnessApp.Server.Features.Exercises.Models
{
    using System.ComponentModel.DataAnnotations;

    public class CreateExerciseRequestModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
