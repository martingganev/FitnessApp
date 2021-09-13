namespace FitnessApp.Server.Features.Exercises
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Exercises.Models;
    using FitnessApp.Server.Infrastructure.Services;

    public interface IExerciseService
    {
        Task<IEnumerable<ExerciseListingModel>> AllExercises();

        Task<IEnumerable<ExercisesByNamesModel>> AllExercisesByNames();

        Task<int> Create(CreateExerciseRequestModel model);

        Task<Result> Delete(int id);

        Task<ExerciseDetailsModel> Details(int id);

        Task<Result> Update(int id, UpdateExerciseRequestModel model);
    }
}
