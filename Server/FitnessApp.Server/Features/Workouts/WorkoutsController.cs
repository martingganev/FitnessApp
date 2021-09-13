using FitnessApp.Server.Infrastructure.Extensions;

namespace FitnessApp.Server.Features.Workouts
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Workouts.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Infrastructure.GlobalConstants;

    public class WorkoutsController : ApiController
    {
        private readonly IWorkoutService workouts;
        private readonly ICurrentUserService currentUser;

        public WorkoutsController(IWorkoutService workouts, ICurrentUserService currentUser)
        {
            this.workouts = workouts;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route(nameof(AllWorkouts))]
        [AllowAnonymous]
        public async Task<IEnumerable<WorkoutListingModel>> AllWorkouts()
            => this.workouts.AllWorkouts(this.User.GetId());
        
        [HttpGet]
        [Route(nameof(AllWorkoutsByNames))]
        [Authorize]
        public async Task<IEnumerable<AllWorkoutsByNamesModel>> AllWorkoutsByNames()
            => await this.workouts.AllWorkoutsByNames();

        [HttpPost]
        [Route(nameof(Create))]
        [Authorize]
        public async Task<ActionResult> Create(CreateWorkoutRequestModel model)
        {
            var id = await this.workouts.Create(model, this.currentUser.GetId());

            return Created(nameof(this.Create), id);
        }

        [HttpPost]
        [Route(nameof(AddWorkoutToUser))]
        [Authorize]
        public async Task<Result> AddWorkoutToUser(AddWorkoutToUserRequestModel model)
            => await this.workouts.AddWorkoutToUser(model.Id, this.currentUser.GetId());

        [HttpGet]
        [Route(Id)]
        [AllowAnonymous]
        public async Task<WorkoutDetailsModel> Details(int id)
            => await this.workouts.Details(id, this.User.GetId());

        [HttpPut]
        [Route(Id)]
        [Authorize]
        public async Task<ActionResult> Update(int id, UpdateWorkoutRequestModel model)
        {
            var result = await this.workouts.Update(id, this.currentUser.GetId(), model);

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
            var result = await this.workouts.Delete(id, this.User.GetId());
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
        
        [HttpPut]
        [Route(nameof(DeleteWorkoutLog))]
        [Authorize]
        public async Task<ActionResult> DeleteWorkoutLog(DeleteWorkoutLogRequestModel model)
        {
            var result = await this.workouts.DeleteLoggedWorkout(model.WorkoutId, model.DateLogged, this.User.GetId());
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
