namespace FitnessApp.Server.Features.Workouts.Models
{
    public class ExerciseInfoModel
    {
        public int Id { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public int Sets { get; set; }

        public int Repetitions { get; set; }
    }
}
