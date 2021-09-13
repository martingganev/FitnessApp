
namespace FitnessApp.Server.Features.Identity
{
    using Microsoft.IdentityModel.Tokens;
    using System;
    using System.IdentityModel.Tokens.Jwt;
    using System.Security.Claims;
    using System.Text;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Identity.Models;
    using FitnessApp.Server.Data;
    using System.Linq;
    using FitnessApp.Server.Data.Models;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Options;
    
    using static Infrastructure.GlobalConstants;
    public class IdentityService : IIdentityService
    {
        private readonly FitnessAppDbContext context;
        private RoleManager<IdentityRole> roleManager;
        private readonly UserManager<User> userManager;
        private SignInManager<User> signInManager;
        private AppSettings appSettings;

        public IdentityService(FitnessAppDbContext context, UserManager<User> userManager, RoleManager<IdentityRole> roleManager, IOptions<AppSettings> appSettings, SignInManager<User> signInManager)
        {
            this.context = context;
            this.userManager = userManager;
            this.roleManager = roleManager;
            this.appSettings = appSettings.Value;
            this.signInManager = signInManager;
        } 
        
        public async Task<DashboardInfoResponseModel> GetUserDashboardInfo(string userId)
        {
            var response = new DashboardInfoResponseModel();
            var user = this.context.Users.FirstOrDefault(u => u.Id == userId);
            if (user == null)
            {
                response.Error = "Could not find user.";
                return response;
            }

            var trainingDays = user.WorkoutSchedule.Split(",", StringSplitOptions.RemoveEmptyEntries).Select(v => int.Parse(v)).ToList();
            var currentDayOfWeek = DateTime.UtcNow.DayOfWeek == DayOfWeek.Sunday ? 7 : (int)DateTime.UtcNow.DayOfWeek;
            if (!trainingDays.Contains(currentDayOfWeek))
            {
                response.IsRestDay = true;
            }

            response.TargetCalories = user.DailyCalorieGoal;
            response.TargetCarbs = user.TargetCarbs;
            response.TargetFats = user.TargetFats;
            response.TargetProteins = user.TargetProteins;
            
            var userRecipes = await this.context
                .UsersRecipes
                .Include(ur => ur.Recipe)
                .ThenInclude(r => r.ProductsQuantities)
                .ThenInclude(pq => pq.Product)
                .Where(ur =>
                    ur.UserId == userId && ur.DateLogged.Day == DateTime.UtcNow.Day &&
                    ur.DateLogged.Month == DateTime.UtcNow.Month && ur.DateLogged.Year == DateTime.UtcNow.Year)
                .ToListAsync();

            response.LoggedRecipes = userRecipes
                .Select(ur => new DashboardRecipeInfo
                {
                    RecipeId = ur.RecipeId,
                    DateLogged = ur.DateLogged,
                    Name = ur.Recipe.Name,
                    Calories = ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Calories),
                    Fats = ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Fats),
                    Carbs = ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Carbs),
                    Protein = ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Protein),
                }).ToList();

            response.CurrentCalories = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Calories));
            response.CurrentCarbs = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Carbs));
            response.CurrentFats = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Fats));
            response.CurrentProteins = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Protein));
            response.CurrentSodium = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Sodium));
            response.CurrentSugars = userRecipes
                .Sum(ur => ur.Recipe.ProductsQuantities.Sum(pq => pq.Quantity * pq.Product.Sugar));
            
            var userWorkouts = await this.context
                .UsersWorkouts
                .Include(uw => uw.Workout)
                .ThenInclude(w => w.WorkoutsExcercises)
                .ThenInclude(we => we.Exercise)
                .Where(uw =>
                    uw.UserId == userId && uw.DateLogged.Day == DateTime.UtcNow.Day &&
                    uw.DateLogged.Month == DateTime.UtcNow.Month && uw.DateLogged.Year == DateTime.UtcNow.Year)
                .ToListAsync();

            response.LoggedWorkouts = userWorkouts
                .Select(uw => new DashboardWorkoutInfo
                {
                    WorkoutId = uw.WorkoutId,
                    DateLogged = uw.DateLogged,
                    Name = uw.Workout.Name,
                    TimeTrained = uw.Workout.TimeToFinish,
                    CaloriesBurned = uw.Workout.CaloriesBurned
                }).ToList();

            response.BurnedCalories = userWorkouts.Sum(uw => uw.Workout.CaloriesBurned);
            response.TimeSpentTraining = userWorkouts.Sum(uw => uw.Workout.TimeToFinish);
            
            response.IsSuccess = true;
            return response;
        }

        public async Task<GetUserPersonalInfoResponseModel> GetUserPersonalInfo(string userId)
            => await this.context
                .Users
                .Where(u => u.Id == userId)
                .Select(u => new GetUserPersonalInfoResponseModel
                {
                    Email = u.Email,
                    UserName = u.UserName,
                    Gender = u.Gender,
                    Height = u.Height,
                    Weight = u.Weight,
                    TrainingGoal = u.TrainingGoal,
                    WorkoutSchedule = u.WorkoutSchedule.Split(",", StringSplitOptions.RemoveEmptyEntries),
                    DailyCalorieGoal = u.DailyCalorieGoal
                })
                .FirstOrDefaultAsync();

        public async Task<UpdateUserPersonalInfoResponseModel> UpdateUserPersonalInfo(UpdateUserPersonalInfoRequestModel model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);

            user.UserName = model.UserName;
            user.Email = model.Email;
            user.UserName = model.UserName;
            user.Gender = model.Gender;
            user.Height = model.Height;
            user.Weight = model.Weight;
            user.TrainingGoal = model.TrainingGoal;
            user.WorkoutSchedule = string.Join(',', model.WorkoutSchedule);
            if (user.DailyCalorieGoal != model.DailyCalorieGoal)
            {
                user.DailyCalorieGoal = model.DailyCalorieGoal;
                CalculateTargetMacros(user);
            }
            
            var result = await this.userManager.UpdateAsync(user);
            if (!result.Succeeded)
            {
                return new UpdateUserPersonalInfoResponseModel
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }
            
            await this.signInManager.RefreshSignInAsync(user);
            
            var role = "";
            if(await userManager.IsInRoleAsync(user, AdminRole))
            {
                role = AdminRole;
            }
            
            return new UpdateUserPersonalInfoResponseModel
            {
                IsSuccess = true,
                Token = this.GenerateJwtToken(user.Id, user.UserName, role, this.appSettings.Secret)
            };
        }

        public async Task<ChangePasswordResponseModel> ChangeUserPassword(ChangePasswordRequestModel model, string userId)
        {
            var user = await this.userManager.FindByIdAsync(userId);
            var result =  await this.userManager.ChangePasswordAsync(user, model.OldPassword, model.NewPassword);
            if (!result.Succeeded)
            {
                return new ChangePasswordResponseModel
                {
                    IsSuccess = false,
                    Errors = result.Errors.Select(e => e.Description)
                };
            }

            await this.signInManager.RefreshSignInAsync(user);
            
            var role = "";
            if(await userManager.IsInRoleAsync(user, AdminRole))
            {
                role = AdminRole;
            }
            
            return new ChangePasswordResponseModel
            {
                IsSuccess = true,
                Token = this.GenerateJwtToken(user.Id, user.UserName, role, this.appSettings.Secret)
            };
        }

        public async Task<IdentityResult> Register(RegisterRequestModel model)
        {
            var user = new User
            {
                Email = model.Email,
                UserName = model.UserName,
                DailyCalorieGoal = model.DailyCalorieGoal,
                WorkoutSchedule = string.Join(',', model.WorkoutSchedule),
                Gender = model.Gender,
                Height = model.Height,
                Weight = model.Weight,
                TrainingGoal = model.TrainingGoal
            };

            CalculateTargetMacros(user);

            return await this.userManager.CreateAsync(user, model.Password);
        }
        
        public string GenerateJwtToken(string userId, string userName, string role, string secret)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(secret);

            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[]
                {
                    new Claim(ClaimTypes.NameIdentifier, userId),
                    new Claim(ClaimTypes.Name, userName),
                    new Claim(ClaimTypes.Role, role)
                }),
                Expires = DateTime.UtcNow.AddDays(7),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            var encryptedToken = tokenHandler.WriteToken(token);

            return encryptedToken;
        }
        
        private void CalculateTargetMacros(User user)
        {
            user.TargetProteins = (long)(user.DailyCalorieGoal * 0.3) / 4;
            user.TargetFats = (long)(user.DailyCalorieGoal * 0.2) / 9;
            user.TargetCarbs = (long)(user.DailyCalorieGoal * 0.5) / 4;
        }
    }
}
