namespace FitnessApp.Server.Features.Exercises
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Exercises.Models;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Infrastructure.GlobalConstants;

    [AllowAnonymous]
    public class ExercisesController : ApiController
    {
        private readonly IExerciseService exercises;

        public ExercisesController(IExerciseService exercises)
        {
            this.exercises = exercises;
        }

        [HttpGet]
        [Route(nameof(AllExercises))]
        public async Task<IEnumerable<ExerciseListingModel>> AllExercises()
            => await this.exercises.AllExercises();
        
        [HttpGet]
        [Route(nameof(AllExercisesByNames))]
        public async Task<IEnumerable<ExercisesByNamesModel>> AllExercisesByNames()
            => await this.exercises.AllExercisesByNames();

        [HttpPost]
        [Route(nameof(Create))]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult> Create(CreateExerciseRequestModel model)
        {
            var id = await this.exercises.Create(model);

            return Created(nameof(this.Create), id);
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ExerciseDetailsModel> Details(int id)
            => await this.exercises.Details(id);

        [HttpPut]
        [Route(Id)]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult> Update(int id, UpdateExerciseRequestModel model)
        {

            var result = await this.exercises.Update(id, model);

            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }

        [HttpDelete]
        [Route(Id)]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult> Delete(int id)
        {
            var result = await this.exercises.Delete(id);
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}
