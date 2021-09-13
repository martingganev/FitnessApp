namespace FitnessApp.Server.Features.Exercises.Models
{
    using System.ComponentModel.DataAnnotations;

    public class UpdateExerciseRequestModel
    {
        [Required]
        public string Name { get; set; }

        public string Description { get; set; }
    }
}
