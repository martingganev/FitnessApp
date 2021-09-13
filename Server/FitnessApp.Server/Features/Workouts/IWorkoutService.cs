namespace FitnessApp.Server.Features.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Infrastructure.Services;
    using Models;

    public interface IWorkoutService
    {
        IEnumerable<WorkoutListingModel> AllWorkouts(string userId = null);
        
        Task<IEnumerable<AllWorkoutsByNamesModel>> AllWorkoutsByNames();

        Task<int> Create(CreateWorkoutRequestModel model, string userId);

        Task<Result> AddWorkoutToUser(int id, string userId);

        Task<WorkoutDetailsModel> Details(int id, string userId = null);

        Task<Result> Update(int id, string userId, UpdateWorkoutRequestModel model);

        Task<Result> Delete(int id, string userId);

        Task<Result> DeleteLoggedWorkout(int id, DateTime dateLogged, string userId);
    }
}
