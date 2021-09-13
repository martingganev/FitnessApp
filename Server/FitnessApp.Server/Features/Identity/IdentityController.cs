using FitnessApp.Server.Data.Models.Training;

namespace FitnessApp.Server.Features.Identity
{
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.AspNetCore.Mvc;
    using Microsoft.Extensions.Options;
    using FitnessApp.Server.Data;
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Features.Identity.Models;
    using FitnessApp.Server.Data.Models.Eating;
    using FitnessApp.Server.Infrastructure.Extensions;
    using System;
    using Microsoft.EntityFrameworkCore;

    using static Infrastructure.GlobalConstants;
    public class IdentityController : ApiController
    {
        private readonly UserManager<User> userManager;
        private readonly RoleManager<IdentityRole> roleManager;
        private readonly IIdentityService identity;
        private readonly AppSettings appSettings;
        private readonly FitnessAppDbContext context;

        public IdentityController(
            UserManager<User> userManager,
            RoleManager<IdentityRole> roleManager,
            IIdentityService identity,
            IOptions<AppSettings> appSettings,
            FitnessAppDbContext context)
        {
            this.userManager = userManager;
            this.identity = identity;
            this.appSettings = appSettings.Value;
            this.context = context;
            this.roleManager = roleManager;
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Register))]
        public async Task<ActionResult> Register(RegisterRequestModel model)
        {
            var result = await this.identity.Register(model);

            if (!result.Succeeded)
            {
                return BadRequest(result.Errors);
            }

            return Ok();
        }

        [HttpPost]
        [AllowAnonymous]
        [Route(nameof(Login))]
        public async Task<ActionResult<LoginResponseModel>> Login(LoginRequestModel model)
        {
            var user = await this.userManager.FindByNameAsync(model.UserName);
            if (user == null)
            {
                return Unauthorized();
            }

            var passwordValid = await this.userManager.CheckPasswordAsync(user, model.Password);
            if (!passwordValid)
            {
                return Unauthorized();
            }

            var role = "";
            if(await userManager.IsInRoleAsync(user, AdminRole))
            {
                role = AdminRole;
            }
            var token = this.identity.GenerateJwtToken(
                user.Id,
                user.UserName,
                role,
                this.appSettings.Secret);

            return new LoginResponseModel
            {
                Token = token
            };
        }

        [HttpGet]
        [Authorize]
        [Route(nameof(GetDashboardInformationForUser))]
        public async Task<ActionResult<DashboardInfoResponseModel>> GetDashboardInformationForUser()
            => await this.identity.GetUserDashboardInfo(this.User.GetId());
        
        [HttpGet]
        [Authorize]
        [Route(nameof(GetUserPersonalInfo))]
        public async Task<ActionResult<GetUserPersonalInfoResponseModel>> GetUserPersonalInfo()
            => await this.identity.GetUserPersonalInfo(this.User.GetId());

        [HttpPost]
        [Authorize]
        [Route(nameof(UpdateUserPersonalInfo))]
        public async Task<UpdateUserPersonalInfoResponseModel> UpdateUserPersonalInfo(UpdateUserPersonalInfoRequestModel model)
            => await this.identity.UpdateUserPersonalInfo(model, this.User.GetId());
        
        [HttpPost]
        [Authorize]
        [Route(nameof(ChangePassword))]
        public async Task<ChangePasswordResponseModel> ChangePassword(ChangePasswordRequestModel model)
            => await this.identity.ChangeUserPassword(model, this.User.GetId());

        [HttpGet]
        [Route(nameof(GetProducts))]
        public async Task<ActionResult<List<Tuple<int, string>>>> GetProducts()
            => await this.context.Products.Select(p => new Tuple<int, string>(p.Id, p.Name)).ToListAsync();

        [HttpGet]
        [Authorize(Roles = AdminRole)]
        [Route(nameof(GetProductsAdmin))]
        public async Task<ActionResult<List<string>>> GetProductsAdmin()
            => await this.context.Products.Select(p => p.Name).ToListAsync();

        [HttpGet]
        [Route(nameof(GetUsersRecipes))]
        public async Task<List<UserRecipe>> GetUsersRecipes()
            => await this.context.Users.Include(u => u.UsersRecipes).Where(u => u.Id == this.User.GetId()).SelectMany(u => u.UsersRecipes).ToListAsync();
    }
}
