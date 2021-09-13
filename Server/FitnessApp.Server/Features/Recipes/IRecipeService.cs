
namespace FitnessApp.Server.Features.Recipes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Recipes.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using System;

    public interface IRecipeService
    {
        IEnumerable<RecipeListingModel> AllRecipes(string userId = null);

        Task<Result> DeleteLoggedRecipe(int id, DateTime dateLogged, string userId);

        Task<IEnumerable<AllRecipesByNamesModel>> AllRecipesByNames();

        Task<int> Create(CreateRecipeRequestModel model, string userId);

        Task<Result> AddRecipeToUser(int id, string userId);

        Task<RecipeDetailsModel> Details(int id, string userId = null);

        Task<Result> Update(int id, string userId, UpdateRecipeRequestModel model);

        Task<Result> Delete(int id, string userId);

    }
}
