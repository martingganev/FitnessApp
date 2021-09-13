namespace FitnessApp.Server.Features.Recipes
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FitnessApp.Server.Data;
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Data.Models.Eating;
    using FitnessApp.Server.Features.Recipes.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using static Infrastructure.GlobalConstants;

    public class RecipeService : IRecipeService
    {
        private readonly UserManager<User> userManager;
        private readonly FitnessAppDbContext context;

        public RecipeService(FitnessAppDbContext context, UserManager<User> userManager)
        {
            this.userManager = userManager;
            this.context = context;
        }

        public IEnumerable<RecipeListingModel> AllRecipes(string userId = null)
            => this.context
                .Recipes
                .Include(r => r.User)
                .Include(r => r.ProductsQuantities)
                .ThenInclude(pq => pq.Product)
                .ToList()
                .Select(async r => new RecipeListingModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Difficulty = Enum.GetName(r.Difficulty),
                    Photo = r.Photo,
                    TimeToFinish = r.TimeToFinish,
                    Products = r.ProductsQuantities
                        .Select(pq => new ProductQuantityListingModel
                        {
                            ProductName = pq.Product.Name,
                            Quantity = pq.Quantity
                        }),
                    IsMine = r.UserId == userId,
                    IsAdmin = await userManager.IsInRoleAsync(r.User, AdminRole),
                    TotalCalories = r.ProductsQuantities.Sum(pq =>
                        4 * pq.Product.Carbs + 4 * pq.Product.Protein + 9 * pq.Product.Fats)
                })
                .Select(r => r.Result);

        public async Task<IEnumerable<AllRecipesByNamesModel>> AllRecipesByNames()
            => await this.context
                .Recipes
                .Select(r => new AllRecipesByNamesModel
                {
                    Id = r.Id,
                    Name = r.Name
                })
                .ToListAsync();

        public async Task<int> Create(CreateRecipeRequestModel model, string userId)
        {
            var recipe = new Recipe
            {
                Name = model.Name,
                Description = model.Description,
                TimeToFinish = model.TimeToFinish,
                Difficulty = model.Difficulty,
                NotesAndTips = model.NotesAndTips,
                Photo = model.Photo,
                UserId = userId
            };
            
            recipe.ProductsQuantities = model.Products.Select(p => new ProductRecipe
            {
                ProductId = p.ProductId,
                RecipeId = recipe.Id,
                Quantity = p.Quantity
            })
            .ToList();

            this.context.Add(recipe);

            await this.context.SaveChangesAsync();

            return recipe.Id;
        }
        
        public async Task<Result> AddRecipeToUser(int id, string userId)
        {
            var recipe = await this.context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return "Recipe Not Found.";
            }

            var user = await this.context.Users.FirstOrDefaultAsync(u => u.Id == userId);
            if (user == null)
            {
                return "User Not Found.";
            }

            var userRecipe = new UserRecipe
            {
                RecipeId = recipe.Id,
                UserId = user.Id,
                DateLogged = DateTime.UtcNow
            };

            this.context.UsersRecipes.Add(userRecipe);
            
            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<Result> Delete(int id, string userId)
        {
            var recipe = await this.context.Recipes.FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return "Recipe Not Found.";
            }

            recipe.IsDeleted = true;
            recipe.DeletedOn = DateTime.UtcNow;
            recipe.DeletedBy = userId;
            this.context.Recipes.Update(recipe);

            await this.context.SaveChangesAsync();
            return true;
        }
        
        public async Task<Result> DeleteLoggedRecipe(int id, DateTime dateLogged, string userId)
        {
            var userRecipe = await this.context.UsersRecipes.FirstOrDefaultAsync(r => r.RecipeId == id && 
            r.DateLogged == dateLogged && r.UserId == userId);
            if (userRecipe == null)
            {
                return "Recipe log Not Found.";
            }
            
            userRecipe.IsDeleted = true;
            userRecipe.DeletedOn = DateTime.UtcNow;
            userRecipe.DeletedBy = userId;
            this.context.UsersRecipes.Update(userRecipe);

            await this.context.SaveChangesAsync();
            return true;
        }

        public async Task<RecipeDetailsModel> Details(int id, string userId = null)
            => await this.context
                .Recipes
                .Where(r => r.Id == id)
                .Include(r => r.ProductsQuantities)
                .ThenInclude(pq => pq.Product)
                .Select(r => new RecipeDetailsModel
                {
                    Id = r.Id,
                    Name = r.Name,
                    Description = r.Description,
                    NotesAndTips = r.NotesAndTips,
                    Difficulty = Enum.GetName(r.Difficulty),
                    Photo = r.Photo,
                    TimeToFinish = r.TimeToFinish,
                    Products = r.ProductsQuantities
                        .Select(pq => new ProductQuantityListingModel
                        {
                            ProductName = pq.Product.Name,
                            Quantity = pq.Quantity
                        }),
                    IsMine = r.UserId == userId,
                    TotalCalories = r.ProductsQuantities.Sum(pq => 4 * pq.Product.Carbs + 4 * pq.Product.Protein + 9 * pq.Product.Fats)
                })
                .FirstOrDefaultAsync();

        public async Task<Result> Update(int id, string userId, UpdateRecipeRequestModel model)
        {
            var recipe = await this.context.Recipes.Include(r => r.ProductsQuantities).FirstOrDefaultAsync(r => r.Id == id);
            if (recipe == null)
            {
                return "Recipe Not Found.";
            }

            recipe.Name = model.Name;
            recipe.Description = model.Description;
            recipe.TimeToFinish = model.TimeToFinish;
            recipe.Difficulty = model.Difficulty;
            recipe.NotesAndTips = model.NotesAndTips;
            recipe.Photo = model.Photo;
            recipe.UserId = userId;

            var newProductIds = model.Products.Select(p => p.ProductId).ToList();
            recipe.ProductsQuantities = recipe.ProductsQuantities.Where(pq => newProductIds.Contains(pq.ProductId)).ToList();
            foreach(var currentProductQuantityModel in model.Products)
            {
                var dbProductQuantity = recipe.ProductsQuantities.FirstOrDefault(pq => pq.ProductId == currentProductQuantityModel.ProductId);
                if(dbProductQuantity != null)
                {
                    dbProductQuantity.Quantity = currentProductQuantityModel.Quantity;
                }
                else
                {
                    recipe.ProductsQuantities = recipe.ProductsQuantities.Append(new ProductRecipe
                    {
                        ProductId = currentProductQuantityModel.ProductId,
                        Quantity = currentProductQuantityModel.Quantity,
                        RecipeId = recipe.Id
                    }).ToList();
                }
            }

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
