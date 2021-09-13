namespace FitnessApp.Server.Features.Workouts
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FitnessApp.Server.Data;
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Data.Models.Training;
    using FitnessApp.Server.Features.Workouts.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using static Infrastructure.GlobalConstants;

    public class WorkoutService : IWorkoutService
    {
        private readonly UserManager<User> userManager;
        private readonly FitnessAppDbContext context;

        public WorkoutService(FitnessAppDbContext context, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IEnumerable<WorkoutListingModel> AllWorkouts(string userId = null)
            => this.context
                .Workouts
                .Include(w => w.User)
                .Include(w => w.WorkoutsExcercises)
                .ThenInclude(we => we.Exercise)
                .ToList()
                .Select(async w => new WorkoutListingModel
                {
                    Name = w.Name,
                    Description = w.Description,
                    Id = w.Id,
                    Exercises = w.WorkoutsExcercises.Select(we =>
                        new ExerciseInfoModel
                        {
                            Id = we.ExerciseId,
                            Name = we.Exercise.Name,
                            Description = we.Exercise.Description,
                            Sets = we.Sets,
                            Repetitions = we.Repetitions
                        }).ToList(),
                    Username = w.User.UserName,
                    TimeToFinish = w.TimeToFinish,
                    Difficulty = Enum.GetName(w.Difficulty),
                    CaloriesBurned = w.CaloriesBurned,
                    Photo = w.Photo,
                    IsMine = w.UserId == userId,
                    IsAdmin = await userManager.IsInRoleAsync(w.User, AdminRole)
                })
                .Select(w => w.Result);
        
        public async Task<IEnumerable<AllWorkoutsByNamesModel>> AllWorkoutsByNames()
            => await this.context
                .Workouts
                .Select(r => new AllWorkoutsByNamesModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

        public async Task<int> Create(CreateWorkoutRequestModel model, string userId)
        {
            var workout = new Workout
            {
                Name = model.Name,
                Description = model.Description,
                TimeToFinish = model.TimeToFinish,
                Difficulty = model.Difficulty,
                Photo = model.Photo,
                UserId = userId,
                CaloriesBurned = model.CaloriesBurned
            };

            workout.WorkoutsExcercises = model
                .Exercises
                .Select(m => 
                    new WorkoutExercise
                    {
                        ExerciseId = m.ExerciseId,
                        WorkoutId = workout.Id,
                        Sets = m.Sets,
                        Repetitions = m.Repetitions
                    })
            .ToList();

            this.context.Add(workout);

            await this.context.SaveChangesAsync();

            return workout.Id;
        }

        public async Task<Result> Delete(int id, string userId)
        {
            var workout = await this.context.Workouts.FirstOrDefaultAsync(r => r.Id == id);
            if (workout == null)
            {
                return "Workout Not Found.";
            }
            
            workout.IsDeleted = true;
            workout.DeletedOn = DateTime.UtcNow;
            workout.DeletedBy = userId;
            this.context.Workouts.Update(workout);

            await this.context.SaveChangesAsync();

            return true;
        }
        
        public async Task<Result> DeleteLoggedWorkout(int id, DateTime dateLogged, string userId)
        {
            var userWorkout = await this.context.UsersWorkouts.FirstOrDefaultAsync(r => r.WorkoutId == id && r.DateLogged == dateLogged && r.UserId == userId);
            if (userWorkout == null)
            {
                return "Workout log Not Found.";
            }
            
            userWorkout.IsDeleted = true;
            userWorkout.DeletedOn = DateTime.UtcNow;
            userWorkout.DeletedBy = userId;
            
            this.context.UsersWorkouts.Update(userWorkout);
            await this.context.SaveChangesAsync();
            
            return true;
        }

        public async Task<Result> AddWorkoutToUser(int id, string userId)
        {
            var workout = await this.context.Workouts.FirstOrDefaultAsync(r => r.Id == id);
            if (workout == null)
            {
                return "Recipe Not Found.";
            }

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return "User Not Found.";
            }

            var userWorkout = new UserWorkout
            {
                WorkoutId = workout.Id,
                UserId = user.Id,
                DateLogged = DateTime.UtcNow
            };

            this.context.UsersWorkouts.Add(userWorkout);

            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<WorkoutDetailsModel> Details(int id, string userId = null)
            => await this.context
                .Workouts
                .Include(w => w.User)
                .Include(w => w.WorkoutsExcercises)
                .Where(w => w.Id == id)
                .Select(w => new WorkoutDetailsModel
                {
                    Name = w.Name,
                    Description = w.Description,
                    Id = w.Id,
                    Exercises = w.WorkoutsExcercises.Select(we => 
                        new ExerciseInfoModel
                        {
                            Id = we.ExerciseId, 
                            Name = we.Exercise.Name, 
                            Description = we.Exercise.Description, 
                            Sets = we.Sets, 
                            Repetitions = we.Repetitions
                        }).ToList(),
                    Username = w.User.UserName,
                    TimeToFinish = w.TimeToFinish,
                    Difficulty = Enum.GetName(w.Difficulty),
                    CaloriesBurned = w.CaloriesBurned,
                    Photo = w.Photo,
                    IsMine = w.UserId == userId
                })
                .FirstOrDefaultAsync();

        public async Task<Result> Update(int id, string userId, UpdateWorkoutRequestModel model)
        {
            var workout = await this.context.Workouts.Include(w => w.WorkoutsExcercises).FirstOrDefaultAsync(w => w.Id == id);
            if (workout == null)
            {
                return "Workout Not Found.";
            }

            workout.Name = model.Name;
            workout.Description = model.Description;
            workout.TimeToFinish = model.TimeToFinish;
            workout.Difficulty = model.Difficulty;
            workout.Photo = model.Photo;
            workout.UserId = userId;

            var newExerciseIds = model.Exercises.Select(p => p.ExerciseId).ToList();
            workout.WorkoutsExcercises = workout.WorkoutsExcercises.Where(pq => newExerciseIds.Contains(pq.ExerciseId)).ToList();
            foreach(var currentExercisesSetsRepetitionsModel in model.Exercises)
            {
                var dbWorkoutsExercisesQuantity = workout.WorkoutsExcercises.FirstOrDefault(pq => pq.ExerciseId == currentExercisesSetsRepetitionsModel.ExerciseId);
                if(dbWorkoutsExercisesQuantity != null)
                {
                    dbWorkoutsExercisesQuantity.Sets = currentExercisesSetsRepetitionsModel.Sets;
                    dbWorkoutsExercisesQuantity.Repetitions = currentExercisesSetsRepetitionsModel.Repetitions;
                }
                else
                {
                    workout.WorkoutsExcercises = workout.WorkoutsExcercises.Append(new WorkoutExercise
                    {
                        WorkoutId = workout.Id,
                        Sets = currentExercisesSetsRepetitionsModel.Sets,
                        Repetitions = currentExercisesSetsRepetitionsModel.Repetitions,
                        ExerciseId = currentExercisesSetsRepetitionsModel.ExerciseId
                    }).ToList();
                }
            }

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
