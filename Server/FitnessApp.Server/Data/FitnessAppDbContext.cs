namespace FitnessApp.Server.Data
{
    using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
    using Microsoft.EntityFrameworkCore;
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Data.Models.Eating;
    using FitnessApp.Server.Data.Models.Training;
    using FitnessApp.Server.Infrastructure.Services;
    using System.Linq;
    using FitnessApp.Server.Data.Models.Base;
    using System;
    using System.Threading.Tasks;
    using System.Threading;

    public class FitnessAppDbContext : IdentityDbContext<User>
    {
        private readonly ICurrentUserService currentUser;

        public FitnessAppDbContext(
            DbContextOptions<FitnessAppDbContext> options,
            ICurrentUserService currentUser)
                : base(options)
            => this.currentUser = currentUser;

        public DbSet<Product> Products { get; set; }

        public DbSet<Recipe> Recipes { get; set; }

        public DbSet<ProductRecipe> ProductsRecipes { get; set; }

        public DbSet<Exercise> Exercises { get; set; }

        public DbSet<Workout> Workouts { get; set; }

        public DbSet<WorkoutExercise> WorkoutsExercises { get; set; }

        public DbSet<UserRecipe> UsersRecipes { get; set; }

        public DbSet<UserWorkout> UsersWorkouts { get; set; }


        public override int SaveChanges(bool acceptAllChangesOnSuccess)
        {
            this.ApplyAuditInformation();

            return base.SaveChanges(acceptAllChangesOnSuccess);
        }

        public override Task<int> SaveChangesAsync(bool acceptAllChangesOnSuccess, CancellationToken cancellationToken = new CancellationToken())
        {
            this.ApplyAuditInformation();

            return base.SaveChangesAsync(acceptAllChangesOnSuccess, cancellationToken);
        }

        protected override void OnModelCreating(ModelBuilder builder)
        {
            builder
                .Entity<Recipe>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasMany(r => r.ProductsQuantities)
                .WithOne(pq => pq.Recipe)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<Recipe>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasMany(r => r.UsersRecipes)
                .WithOne(u => u.Recipe)
                .HasForeignKey(ur => ur.RecipeId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.UsersRecipes)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserRecipe>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasKey(ur => new { ur.UserId, ur.RecipeId, ur.DateLogged });

            builder
                .Entity<ProductRecipe>()
                .HasKey(pr => new { pr.RecipeId, pr.ProductId });

            builder
                .Entity<Workout>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasMany(r => r.WorkoutsExcercises)
                .WithOne(u => u.Workout);

            builder
                .Entity<Workout>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasMany(r => r.UsersWorkouts)
                .WithOne(uw => uw.Workout)
                .HasForeignKey(r => r.WorkoutId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<User>()
                .HasMany(u => u.UsersWorkouts)
                .WithOne(ur => ur.User)
                .HasForeignKey(ur => ur.UserId)
                .OnDelete(DeleteBehavior.Restrict);

            builder
                .Entity<UserWorkout>()
                .HasQueryFilter(r => !r.IsDeleted)
                .HasKey(uw => new { uw.UserId, uw.WorkoutId, uw.DateLogged });

            builder
                .Entity<WorkoutExercise>()
                .HasKey(we => new { we.WorkoutId, we.ExerciseId });

            base.OnModelCreating(builder);
        }

        private void ApplyAuditInformation()
            => this.ChangeTracker
                .Entries()
                .ToList()
                .ForEach(entry =>
                {
                    var userName = this.currentUser.GetUserName();

                    if (entry.Entity is IDeletableEntity deletableEntity)
                    {
                        if (entry.State == EntityState.Deleted)
                        {
                            deletableEntity.DeletedOn = DateTime.UtcNow;
                            deletableEntity.DeletedBy = userName;
                            deletableEntity.IsDeleted = true;

                            entry.State = EntityState.Modified;

                            return;
                        }
                    }

                    if (entry.Entity is IEntity entity)
                    {
                        if (entry.State == EntityState.Added)
                        {
                            entity.CreatedOn = DateTime.UtcNow;
                            entity.CreatedBy = userName;
                        }
                        else if (entry.State == EntityState.Modified)
                        {
                            entity.ModifiedOn = DateTime.UtcNow;
                            entity.ModifiedBy = userName;
                        }
                    }
                });
    }
}