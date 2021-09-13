using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using FitnessApp.Server.Data;
using FitnessApp.Server.Data.Models.Training;
using FitnessApp.Server.Features.Exercises.Models;
using FitnessApp.Server.Infrastructure.Services;
using Microsoft.EntityFrameworkCore;

namespace FitnessApp.Server.Features.Exercises
{
    public class ExerciseService : IExerciseService
    {
        private readonly FitnessAppDbContext context;

        public ExerciseService(FitnessAppDbContext context)
        {
            this.context = context;
        }

        public async Task<IEnumerable<ExerciseListingModel>> AllExercises()
            => await this.context
                .Exercises
                .Select(e => new ExerciseListingModel
                {
                    Id = e.Id,
                    Name = e.Name,
                    Description = e.Description
                })
                .ToListAsync();
        
        public async Task<IEnumerable<ExercisesByNamesModel>> AllExercisesByNames()
            => await this.context
                .Exercises
                .Select(e => new ExercisesByNamesModel
                {
                    Id = e.Id,
                    Name = e.Name,
                })
                .ToListAsync();

        public async Task<int> Create(CreateExerciseRequestModel model)
        {
            var exercise = new Exercise
            {
                Name = model.Name,
                Description = model.Description
            };

            this.context.Add(exercise);

            await this.context.SaveChangesAsync();

            return exercise.Id;
        }

        public async Task<Result> Delete(int id)
        {
            var exercise = await this.context.Exercises.FirstOrDefaultAsync(p => p.Id == id);
            if (exercise == null)
            {
                return "Exercise Not Found.";
            }

            this.context.Exercises.Remove(exercise);

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<ExerciseDetailsModel> Details(int id)
            => await this.context
                .Exercises
                .Where(p => p.Id == id)
                .Select(p => new ExerciseDetailsModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Description = p.Description
                })
                .FirstOrDefaultAsync();

        public async Task<Result> Update(int id, UpdateExerciseRequestModel model)
        {
            var exercise = await this.context.Exercises.FirstOrDefaultAsync(p => p.Id == id);
            if (exercise == null)
            {
                return "Exercise Not Found.";
            }

            exercise.Name = model.Name;
            exercise.Description = model.Description;

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
