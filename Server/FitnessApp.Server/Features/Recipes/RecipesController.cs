using System;
using FitnessApp.Server.Infrastructure.Extensions;

namespace FitnessApp.Server.Features.Recipes
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Recipes.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Infrastructure.GlobalConstants;

    public class RecipesController : ApiController
    {
        private readonly IRecipeService recipes;
        private readonly ICurrentUserService currentUser;

        public RecipesController(IRecipeService recipeService, ICurrentUserService currentUser)
        {
            this.recipes = recipeService;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route(nameof(AllRecipes))]
        [AllowAnonymous]
        public async Task<IEnumerable<RecipeListingModel>> AllRecipes()
            => this.recipes.AllRecipes(this.User.GetId());
        
        [HttpGet]
        [Route(nameof(AllRecipesByNames))]
        [Authorize]
        public async Task<IEnumerable<AllRecipesByNamesModel>> AllRecipesByNames()
            => await this.recipes.AllRecipesByNames();

        [HttpPost]
        [Route(nameof(Create))]
        [Authorize]
        public async Task<ActionResult> Create(CreateRecipeRequestModel model)
        {
            var id = await this.recipes.Create(model, this.currentUser.GetId());
            return Created(nameof(this.Create), id);
        }
        
        [HttpPost]
        [Route(nameof(AddRecipeToUser))]
        [Authorize]
        public async Task<Result> AddRecipeToUser(AddRecipeToUserRequestModel model)
            => await this.recipes.AddRecipeToUser(model.Id, this.currentUser.GetId());

        [HttpGet]
        [Route(Id)]
        [AllowAnonymous]
        public async Task<RecipeDetailsModel> Details(int id)
            => await this.recipes.Details(id, this.User.GetId());

        [HttpPut]
        [Route(Id)]
        [Authorize]
        public async Task<ActionResult> Update(int id, UpdateRecipeRequestModel model)
        {
            var result = await this.recipes.Update(id, this.currentUser.GetId(), model);

            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete]
        [Route(Id)]
        [Authorize]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.recipes.Delete(id, this.User.GetId());
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpPut]
        [Route(nameof(DeleteRecipeLog))]
        [Authorize]
        public async Task<ActionResult> DeleteRecipeLog(DeleteRecipeLogRequestModel model)
        {
            var result = await this.recipes.DeleteLoggedRecipe(model.RecipeId, model.DateLogged, this.User.GetId());
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
