using System.Runtime.CompilerServices;

namespace FitnessApp.Server.Infrastructure.Extensions
{
    using Microsoft.AspNetCore.Builder;
    using Microsoft.AspNetCore.Identity;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.DependencyInjection;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FitnessApp.Server.Data;
    using FitnessApp.Server.Data.Models;
    using FitnessApp.Server.Data.Models.Eating;
    using FitnessApp.Server.Data.Models.Training;

    public static class ApplicationBuilderExtensions
    {
        public static IApplicationBuilder UseSwaggerUI(this IApplicationBuilder app)
        => app
            .UseSwagger()
            .UseSwaggerUI(options =>
            {
                options.SwaggerEndpoint("/swagger/v1/swagger.json", "FitnessApp API");
                options.RoutePrefix = string.Empty;
            });

        public static void ApplyMigrations(this IApplicationBuilder app)
        {
            using var services = app.ApplicationServices.CreateScope();

            var dbContext = services.ServiceProvider.GetService<FitnessAppDbContext>();

            dbContext.Database.Migrate();
        }

        public async static Task SeedDataToDbIfEmpty(this IServiceScope services)
        {
            var context = services.ServiceProvider.GetService<FitnessAppDbContext>();

            if (!context.Users.Any())
            {
                await SeedIdentityData(services);
            }


            if (!context.Products.Any())
            {
                context.Products.AddRange(GetProductsData());
            }

            if (!context.Exercises.Any())
            {
                context.Exercises.AddRange(GetExercisesData());
            }
            
            await context.SaveChangesAsync();

            var user = await context.Users.FirstOrDefaultAsync();
            if (!context.Recipes.Any())
            {
                context.Recipes.AddRange(GetRecipesData(user, await context.Products.Take(20).ToListAsync()));
                await context.SaveChangesAsync();
            }
            
            if (!context.Workouts.Any())
            {
                context.Workouts.AddRange(GetWorkoutsData(user, await context.Exercises.Take(4).ToListAsync()));
                await context.SaveChangesAsync();
            }

        }

        private async static Task SeedIdentityData(IServiceScope services)
        {
            var roleManager = services.ServiceProvider.GetService<RoleManager<IdentityRole>>();

            await roleManager.CreateAsync(new IdentityRole { Name = "Admin", NormalizedName = "ADMIN" });
            var userManager = services.ServiceProvider.GetService<UserManager<User>>();

            var user = CreateUser("user@user.com", "user");
            
            await userManager.CreateAsync(user, "user");

            await userManager.AddToRoleAsync(user, "Admin");
            await userManager.CreateAsync(CreateUser("user2@user.com", "user2"), "user2");
        }

        private static User CreateUser(string email, string username)
            => new User
            {
                Email = email,
                UserName = username,
                DailyCalorieGoal = 1500,
                Weight = 100,
                Height = 180,
                TrainingGoal = TrainingType.BurnFat,
                WorkoutSchedule = "1,3,5"
            };

        private static IEnumerable<Exercise> GetExercisesData()
        {
            var exercises = new HashSet<Exercise>();
            exercises.Add(new Exercise
            {
                Name = "Threadmill - Running",
                Description = "test"
            });
            exercises.Add(new Exercise
            {
                Name = "Dumbbell Bench Press",
                Description = "test2"
            });
            exercises.Add(new Exercise
            {
                Name = "Threadmill - Walking",
                Description = "test3"
            });
            exercises.Add(new Exercise
            {
                Name = "Dumbbell Flyes",
                Description = "test4"
            });
            return exercises;
        }

        private static IEnumerable<Product> GetProductsData()
        {
            var products = new HashSet<Product>();
            products.Add(new Product
            {
                Name = "Egg",
                Carbs = 0,
                Fats = 4,
                Protein = 6,
                Sodium = 62,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Banana",
                Carbs = 22,
                Fats = 0,
                Protein = 2,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Milk",
                Carbs = 5,
                Fats = 3,
                Protein = 3,
                Sodium = 43,
                Sugar = 5
            });
            products.Add(new Product
            {
                Name = "Yogurt",
                Carbs = 4,
                Fats = 2,
                Protein = 3,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Cheese",
                Carbs = 3,
                Fats = 26,
                Protein = 16,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Flour",
                Carbs = 77,
                Fats = 1,
                Protein = 11,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Onion",
                Carbs = 10,
                Fats = 0,
                Protein = 1,
                Sodium = 2,
                Sugar = 2
            });
            products.Add(new Product
            {
                Name = "Carrot",
                Carbs = 10,
                Fats = 0,
                Protein = 1,
                Sodium = 69,
                Sugar = 4
            });
            products.Add(new Product
            {
                Name = "Oil",
                Carbs = 0,
                Fats = 92,
                Protein = 0,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Chicken",
                Carbs = 0,
                Fats = 12,
                Protein = 21,
                Sodium = 85,
                Sugar = 1
            });
            products.Add(new Product
            {
                Name = "Pork loin",
                Carbs = 0,
                Fats = 10,
                Protein = 29,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Pork breasts",
                Carbs = 0,
                Fats = 17,
                Protein = 19,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Minced meat",
                Carbs = 2,
                Fats = 22,
                Protein = 15,
                Sodium = 0,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Almond raw",
                Carbs = 20,
                Fats = 51,
                Protein = 21,
                Sodium = 1,
                Sugar = 5
            });
            products.Add(new Product
            {
                Name = "Orange",
                Carbs = 12,
                Fats = 0,
                Protein = 1,
                Sodium = 0,
                Sugar = 9
            });
            products.Add(new Product
            {
                Name = "Butter",
                Carbs = 1,
                Fats = 83,
                Protein = 1,
                Sodium = 0,
                Sugar = 1
            });
            products.Add(new Product
            {
                Name = "Trout",
                Carbs = 0,
                Fats = 5,
                Protein = 16,
                Sodium = 0,
                Sugar = 1
            });
            products.Add(new Product
            {
                Name = "Potato raw",
                Carbs = 17,
                Fats = 0,
                Protein = 2,
                Sodium = 6,
                Sugar = 0
            });
            products.Add(new Product
            {
                Name = "Potato cooked",
                Carbs = 41,
                Fats = 15,
                Protein = 3,
                Sodium = 0,
                Sugar = 0
            });
            return products;
        }

        private static IEnumerable<Recipe> GetRecipesData(User user, List<Product> products)
        {
            var recipes = new List<Recipe>();
            recipes.Add(new Recipe
            {
                Name = "PARMESAN OVEN ROASTED CAULIFLOWER",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Easy,
                Photo = "data:image/jpeg;base64,/9j/4AAQSkZJRgABAQAAAQABAAD/2wCEAAkGBxMUEhYUFBQWFhYYGSEZGBkZGR4ZGRweHxkZGRkYGxkaHioiGR4nHBgaIzQjJystMDAwGCE2OzYuOiovMC0BCwsLDw4PGxERHC0nIigtMjQxODAvLy04Mi8xLzEyLzoxLy8vLzgyMTIvLy8vMS8vMDgvMS8tLy8xLy8vMS8vL//AABEIAJ8BPgMBIgACEQEDEQH/xAAcAAABBQEBAQAAAAAAAAAAAAAFAAEEBgcDAgj/xAA/EAACAQIEAwYDBgUCBgMBAAABAhEAAwQSITEFQVEGEyJhcYEykaEHFEJSscEjYtHh8ILxFSQzQ3KSssLSFv/EABoBAAIDAQEAAAAAAAAAAAAAAAMEAAIFAQb/xAAvEQACAgEEAQIFBAICAwAAAAABAgADEQQSITFBE1EFImFxkYGx4fAywaHRI0JS/9oADAMBAAIRAxEAPwDWqVKlXZyPSpUOucasKwVmgkgAEEbkj9R+nWqswXsyZhKlXhHBEgyKea7JOeKxK21zNtIHzMV1RwRIMjyql9p+Po+HIDFH7zKVBkwurweaxz6kUZ7Ii793XvIHJVAGg5SRuTzoS27n2jrEqGycQ7SLRrQ3FcZt27gtvmUkEyR4YAkmelCOP9pLXc3EtNmcoYg7SDMnlA+pA6xx70QHJHE7kSz4a+txFdTKsAwPkRIrrVU4Dxa4LaBlhQAI35DUE6n33q0W7gYAgyDVaNQlo47EPZQ1YBPme6emrhiMWqaE68h10J325Gjk47gwMyTXMYhcoaYDbTp5xB50OfiDEGBlI168/hI9J10/eomZpOukCYVQMx1B8tGJPrsaGbB4lxWfMO27ykwDXSq3auwS8gxOXTMQRpO5OhJ2jc8tSWwONzATr/NO401Og5mNPXnUWwHuRkx1J9PTU9Fg4qVebl0KCSYgSaQuCpJPRNcjiV15x0rhjcRlQmCZn/aq799cEqgmTrPXoOnrQLtQtXcYp07WZxLOuMTmQPU064u2dA6/OhKTE6ba66fOuqYhCYMA+3+GuC7P8zpoIheaaahI5HwmR0P9alB9JoytmAZcT3NNNVviXa62iFrSm6YJEERpy3nbWqnf7ZXsQhQHu2k6gEZgQSNdNQII669KWs1daA45+0EXAly432hVEfuSrupCtvCzpJMQTPKpXDOKK1i29x1kwrEERmI5x8M7+9ULstigLpt3rko8lwVnNAZhmJbwxrsCSYqxYzE4dsFcWwRoMwB8JmQQdd9ooNeoZgXyOjx9RKK5PMtwNKq52R4oLlsywORFk+hcT8gD70Yw+ND3HQfhCmQdDmE02lysqnPcIGyMyXTUqVGloqalSqSRqVKlUknOlSpV2cioD2msJCsQJB0aYZTyYdYNH6j4pMwiSPTf0qli7lInCMiUfhPEL2Hld0Gw9ddB/m9T+McfvgBMiKHTcEs2oiVGgOp2OunOi93h4AOXUb5TBB+lV3jOOw5S2zMrRJttqoBgRlc/EP8ABEVnlLKlI3GD5Ez84vxlLjEwWEnbKTrI5GdfrVo4Z2se2tq0qgIWMwMkkzoCNtegHKNqq/GWW5ecpqTqFB1jQeL0J13MVyV2QagtEQN/EM235YJXXy50DJA4PMoDLVxbGuzAvcLAtGbqoykjoNl0GhmpfD3R3711zAmQA0jbz+IeU1QHu3sQ6oilpAlQYAPIALJJgcug86t/AuyOMQBQpVdxLieXIkyNfKqGiw8g85zg8x7Selu/8oJH0hvE45UuSjaNrlOhB5iDy9KsXA+KDY/CT8jVQ4jwTEhT3lkuB+JYJHmApJofw/jIttlZtPPQ+jA0vm2mzfjHuB1+k3/Tpur2qc/vNdv3gonfYbgb+Z0qv4viSGSzqDt1j21zA9f6a58vat7jEXHbLOkGFUchHM+dS8Zxa2B4WVtPwmY9Sa1jeXGepWr4KwI3HOfaXNMU+XOASvxJrlLCMwEETMTsdIIkbV3tIx8bqZUQCG121A1EkSdCfSAYqk9ksc1y7cABKldw0QZ01856j9jbWfQZ2kKNSViZXcbkgzHhOkgRHhqI24ZiGr05osKZzJOJiZjxIZ3cbxMldAdV6yBABiKa0rBpGYgHMAT4hqdAAusEMYJnXyApjcWYOsRlAmBoTByydC3Mee2teMCUa4VJHPMFJjwiWBGwmfw811JjWw7ix6k6/wAUNuCVZrhGiEiQOrkaDX19qjjiOLbX+Ev8uUmPeda5YYyzXDGZiSevkPapqGRWfZrLC2FOBCLUoHInJOMEkJeUIWMKwMoT0M6r+lEcNe8tRoRz86FYy0HUqdf8P9KfDYsd2hZoYEpmbQHLESepBHrFN6PVFyVbuCtqC8iGcVYz2yF5jQ9KA4OxyHvRGzxDIdRp5Db16VKtJZbVYU+R/Y0e6n1GDDxO0X+mCp8wTjrkaVV+IYjMYDVauK4C4RCrmB5jX6VC4f2aKtnuKpETlnNJ8/L+1JW1WOdoH8TUovqrTcx5nfsvYuFczscnKdz6HpXjtTx9EW5YAbOQIOw1K6T5ydvOrELmg09ht7VR/tGwfwXiSqhSjZRqeawTou7SdOnSGnqNVG1T95i6m42OWxiUhcQA4C3pckaQQsDeANtPPlvrUdnm5c70gHfQmICnLEa/CB9OtSu7Q3VtW1YAhmL5jlAkiVJMsflUPiPD7Vk5yTdkjKQQoBIOUNGhAGXUk+m9Z4AxiJmcsDby3PERkdsyksskyCJU7Gf96tvAuFvdFxXYNbFvOrf9yRM5TMZSdNh8PlUzshwuxfzXSoYEiDtMAZpjcSBv0qyY3h1gFbaoULTDJsDrqV2YetQf/R/H1h00rEZgLseSt27bLEhrWk+kgewNHuxZJFxzzyj5A/2qq8FNy1jQt0jOGKEgQGBTwkDlIA0qzdlcUqO1mNWZiDy8MCPoaLQRvQt4JH56g14ODLVSpVysXw8xyJB9RWuSAcQs601PTV2SKmp6apJPFKnpV2cjV5Ir3TEVJJExdkMpHUciR9RqKyfiOEADKzfC0ATqCBrr5bfKtX4hbcr4Hyn0BB+dUu52ZUFizHvG/KPCv8pmcx9I96zte+0A4lfTZzhRM4RCtwZSWUNBVvwkiBB3AkDy18qWI4abt5bKpDO2XNqSAxKlpkiAJ8x76nu2XAzYQXkDXEnKeRUxOUjmpgxHMV37H3luY20huarbe4B18GVSevxM3+kUrWxYgiVFRDhWlrYJhLITCooCgeIrJbT4mOhOg5/2qo3e12LN8jv/AIWiMq5fMQANdDzmrjxHFNbRhcGUWxBJ/EzSFGY6RpPuOlV7hfDkKjPlloLEd2IJzgmST/NPqNRFGLGayoAIV4f24KqO+tiJgkSCOREGaMY7huHxlnNaya8wAfY9P2qsYnhZybH4TI+ESRqMskCNGnlmnXehnCeH4izfy4c3LcgjnsTu6gAOQomZ0LAc66G8GdxjleIB45hhZcWypkMxYmQYGgTzjf386g4W41092is55KgJP0q3dtuCm7csWwsXCuQMdywic5H8upPKKsfBOFW8Lb7uyIP43iGc8yTuB0X50vdbXQu5z9h7zQ02ttSsonZJJJPvIfZzgT20CtauKWAbNEEMCdADpMHedl1BmKl8U4kttGuFxmEIFhPEGXKYI5a9BGU+88Gqh25umLakk6lhJJA3nnuS30FC03xBLm2YIMz9XXYFLsczjju0bW7wVQFllDpCkHNqGmBrB/WrxwnEot1EzOJJWGIjxAifDAPigz84O+SsxbEIxJJa4mxgzIAg8q2rDYBGBzk667kR6QdPWiXapNPgdkngfb7xLTgspyZAuMUJUyInXnzj2/rTrjNNz7z/AJ/tXTiV20bipcuLbuH4WfRLkafF+F/1/SK/Br23dk+cjLziNdP70ma2b568kHrH+48LB03Bnb71Ok/T9+f151A41xMILVoqXtOLhvOokWycuSQNTorTFCeJ8T7lyiGCNCQdAZMhTJ2j60P/AOJBjJMGfX6GndJQ6ksfIiN+qAbC+IWu3cTb8dm5KxogIaRHQzBP0prXF8SRnFwsvkqqfOYA8QjbShhxhfQaEbEc/wBPnXi7iiWOQHvNmSdLsDmBs4/N0pwq6judpvWxsFRmWvCdoVujK9/uyOTQg/VQfc0btYohZ+8WXHLb6EMZNZBi3vXHKm2tmDoIJLbiAZ118udDsfhrmHYZWEkTHPrB84qm855PP3Mb9PKkjocnibZxDjTWrTOBOVSZKkKIG4Zo/SslxXaXGXbdy298XLbj8S59zO2mXURsRUFuIYi9Y7huTeL+IcriDCm2JHnOxIEVJ4Vhx3QfJlGyqVKvIMtq24Ohgj57V220qMkzOtZSflnK3xR1tAPdzHkdVzEmCs8tl6b+1SeBcYDtkFhHAJMABiNoEkQACBJ5x7mHxO6oeGT+JHhUhSDI8Mx8B1HP0oRhu8LsyHIEIDeITPMAkidOU8hvQggZSYCaPhOMdzdW0AfHqTAUBySSDBIDbGJ50bbtAqmWIEayTy6+lUDhOLV07vvG8QPiJIAIOhCbCT+Ya6V0THfDbdO9DSJ5Z+ikQ0bwdOXIar+jz3j3jtWtKKFIzgQ83Ee9xBvDYMpGusAwCRy3o3we7GNtHqXHzJFUfCXXEgDQRDHxcxmMLvt57VZeCY1WuWHUzqNRt8U0EMyupPuIkzbmLH3mn3LoBC8yCflE/wDyFQuCGbZPVia8Y67F7/xsMfmy/wD5rzw28LeGDtsAT9TFbRszbz0Af9QueZMu3ybgtry8TnoOQ9SfoDUuoHCbRy52+NzmPvsPYVMt3Adv85UZCSMnzOieqanpUSdnilT0q7ORq8vMGN+XMfKvVMakkrmLxGK2ZIZHDKU1tuojMGJOZSfFGkbe0y2skusMrTqBPPUeTDYio3bXif3fB3roJDZcqEb5m0WJ89fasQtcQxbszi9dRVAFxxcYZ2LAHxTyBP0pLU078HPUPXuVS3iaP9pnEkt4drEqbtwghSfhVTmLkDUawBWddmsV/wA1YuyAGlGM6Q2ayB9Qf9NE+yXYn79avXDecMGyhpzEsANWJ1YcqA4rhF2xdey/guWhAjXc5lZSd9v1oKIiDAlLXG4HyJsHaW2XHiB+PwmVjQEgkMegIA6n3Ht7ZIJCRMhlOUPrmkSubULOmsnadah8B7Q2sfZyIypfSC9t+ZAOxI1UnWQJ0qWYQnOoTLBIBnKANFEiFIGaAuh301irDB5jyMGHEXcIGPhCjRvDCk/ERmDnw8xqu3ODUjhkh2YLGgRfMkAk6GCNBMCDlnWaj5rhhFJDbAGQoGXXTMAZicyqVEn4tjJwWOsWzGYQJ6xJ3kxv51FHMjHic+JkC/ZQ5c2V23k/gE9dZYT5monEsQyBcqsxZgIAk9f23qo43jTvfucRaBZtP93tKPxpmIdh1kmQfKrDa4pZxFsm1c5bjwsOWxEjpNZPxKpjarYyoGP1jGidWBAPOZwxPFza8VxkVJIzO0EanLoFjUD6VVuN8bt4hptmQg3ggGYIIkAkVYuJ9kjiLepuBQsAAKQTHx5ZBJ16+lUmxg2su6NlkECVMqREgjyIM6601ptLUFFn/tAfEL3ClMce8aziO7v23bZLlst/7ST7b1sw4rbW3nZgBHM1huMUP3i9dBU7E33W0oZyYA0J0Ggrms0K6nZk4x+0z6NQK1IIzNBuYuxjr9tWk27eZiIIDchrzAg6Vb24HhBay92oWIgSB9DWI9lsDiL7m7YM5DB8RA65SeuvL/bQk7RXjbGe0FOsgsCREidJHKd+m1KajT2UELVyMYwT19Ydbt2S5x9oB4/gFW+y2PhgaHQLGkCN9PKoNpu7kOQ0+UD2movEbjB2z5lcknX4TzkRuNuse9QbeIJkmCV0g/tW1pEIRQTniZ9hDOSOpYgwGwjprr11EzHlXGxx9rTG2hGdvCCY0gzqW2EE/Sq4/EGGiyxbYDfpy/Wu2C4MbrM3eqGXKSSCd9ApOYBNNYI+mtN+mzqQIfSV5sBPUmcYe7riEZHggMGCl+cEZRoNGHWu/D+FDEubjOQ2UuF/CIAlWPI6+9deJcGvNayC7LEyBCke8NpoJ5x0FV+9cxGGARwCTI8Op36xEc/el309irnHM3EapAFZuCOeODCeDsvafvVEKoyouaZYMZbbzj296vOAXMA1wS5RSxMb9KqnZS13qZmDeEkaiPF8R9YkVaeL3RYyID8IBIOsmNNBrp1g7msvUMd2D3MC7b6jbP8AHPH2nriOCDALesi5bOpET+nP+1VbjnZq1aY3bdwC1bGcWtdGAmNNd431qyf/ANDIhVgfmZDl8+n71DxOILowLAgAmCNJI/wRFcSw1naMwWZXOx3Y+7jf4pdU0JKspY7+GDPh1n2Bq5WPs0yWyoxJtqykMAAVBMGQTBnQa0L7OdqDg7wS4AcOVCG5oCrJMsRzXWOoiaNfaNjHt3LN6O8w7LlPNQ0yCfUbelOFnYj6/t4jdFaPwZV+M9mcRhMt0g3FUx3iQdNhKxIG2xjrXXstbIRGC5UB8J5Ea6gbDWas32cY18R94Ur/AMvGkjw5jIKr7bx5U9nDKmEVF/A5X1AZ4+hBoVg+Xnsc/iUvpVDwZYeKXv4l8/ltIn/sSf3rhjsWALFjUgBWcDcnTKnuagY7FjLfYnQ3iD6W1j9xXPhWKgvibo1J8C9WI8I9FWPmK6bcuRnHufYQJMt/EMQQBbT/AKj6DnlH4mPkKlYWyEUKOX1oTwdSFN+6YZtZOkLy9P8Aap1jEG7qsrb/ADHQt6DkPPf9a1qzn5j2eh7CEBk2lSApUaWnmlSpV2cjUB7YcTvYeyr2QpOaDmBIiCSTGw03nnR41lf2ui6160tvM02iQijMdGMmADoZUe1UcEjgwlYBYZlK7TdoDjHLXbr25ylVAm0sCAQoJnnr158qH3OI3LXhthXtZY8JnwxGs89frUK9wy8q57quik5Rn8JJjkG194rngS1hWuODOyaaecn5ECqnqaDMpUYH48Y95pH2O8Ra33uGuSGJ722DzEAFfPSD70V+1DhGZVxKboIfzQ8z/wCJ19C1ZTw7iF4X/vCHK1s5weXPT0O1bzwXidvGYYXFAKuIZTyb8aGkbsqx/vMzLANxAMwprly0/e2viHTQ+xG39+dXTgnb2xdAGJzK4hS2UkETqCQfCYnXz3ANA+0HDzhMS1uPCfHaJ5oeXqu3y60F4jwz/vWRI/EtWXaw5la7WQzSMbx62krYzPmkG4eYJkqonwjYcs2UVVOO4/E3LiWFQIlwiG3LCVDHoIJGnv6HOzNq3esq0SI08iBqKn8Rwih7bjXIxPsUb98tLerszx1LPqHac7uFsNcw+G2tpDR5+IKxPXwvr1M7ijXHeE4dLQa2sOCIg6sSQMpneZj3qn8EY4jF35YqrQidJWSrDzkn2Y9KPcO4JinHem8veWmJQEZkJUwGYaaT/WoRwMn7/vHtGVK9cyXi+07Wh3bqyvtkIIM+U7+o0qjdo7hVEukCdVuZTMNmZxMc4b6VeuE8UP3i597y97bAAgkrlIkss7SdPagna69ZxFwqiyTbaehI8VuTsDmGk+ddrKq+BzD6hC9Zz/TKXwezAa8x+IyoPIdfWvOHwj4y/wB2ki2D4j1qBjMe7Hu1GUTl/aK0ngODt4HCG/cGwmOZJ+Ffc09VVltzfpMZUOcmB+0mLTh9lcPhiUuGGYjp/MvMt84A8qK4bhd/ubff2ltsUGjELJI8UwZ9iRqOdCOxuS7fvY/EjOLTGF5G5lLSeiou3mR0qBxnH4nHOWvXItycqLooH5SAfEQANDJk+dcvrWw48jzDhARlpYb+CLMFxFsso/6b92xP5ZJVYUjyPi1Owmhdvs3ZhgjM+XRiTmhtSykKTBg+Y0oLOJRCi37uU8gx+Qb4huNoo7wZmxlg2c728VYT+G2aFuCTGcnWT8JY7HKaD6LKPlaVNQP+Mi9mMELSPcKBnzMvinxEQR4V2VV1gzmY/wAtWzhvBVgIVBLr3t78K2xl8RCxqBroRMt0rx2Z4Lms4fLmZjdvd4QfEMjIMuomYBHv71cj2fusrrKr3twm4x1bu1PgRQBEmATNa4YKoEc3KigStX71qAEtdzbEAMyS5EEs2bKSSROikRm3jWomNwqOpzqvz30Gg68j+3TvjcFdsvlVtNc8ABI1he9JGcjSSYHkNqiYS4dRAIEctFJAiCNJiduu3OiEBhiEwGE5di7RzmScurspHwkMyhQecbddRvzl8TuupY6DMZlgT9JH6V14BayHEHUAsoBkbasY6akD2qRjMB5HrpMNXj9Z8txHjMzLEwSBAds37hCK+bMQAI8Ik7gVI7R4mzhxkVYyxLsZPmRpCztOhkjlRe1aGGQ3n+KIUef5vKNp86pXEb93FX1QKzorS35fDrHi00/X0q9Kb8Z/og9uBLR9nlm3iQzXLSOgn4wHiek9QNZHlRriWCxGDRvuyLfw8a4dtSo6Wyfw/wAp25dKj/Z8FBu5BGgzCCJO0mddhG1Wbu7wMzI5g7enlVns2ngcRqlMpM+sfaOzgW7dgWEMjeDpuqiBB3E144fxcs4UfDc1A6ZdduQqH9o2CtWcSWgKLiZo6mfF+0xQzsOq3MSggmNzyXwmd/KetXsQMhdeODFrdxbBlnS6Xtop5ks3nLT9THsDRPBstx1BnIm4666COrHU/KhFjxsQugILDnCzCeumvqYq7dn+FBVBIjpO/qfOqaal7Xx+plFBJkzC4RrpDXfhHwpyHmepo2oivKLFe630QKMCHAxFTUqVWnY1KmpV2cjGoN/CKXLwMxUKW5wCSB6ST86lteUHKWAPSRPyqs9qcdNxLKNctuCPGJCaxvrrH9aG7hRmERCxxMh7c4tmxeIRlznMbayNV1ISOnKgmIxj/ASGZoBiYESCR6jTXqa0Hi3BrT3WuWGzjR3UgyWDAtcafw7mZ0mNKB3eBWrl/wAJIDQQSPFmzEMNDE6Tz0pV9QoyT1G9jKnHt+IIweGGWCYDKRtOxBAqzdjeJ/dLxGYm1dg3ByXSM49D9BQm/ZyOAQMu3oZgH01+pqLbYhUk6LmUzvI8Q+jE/wCnzpTJfozKBIM0j7SOErfw0gEunjtsu46jzBGkelZvctXbHjNu4tsmFNxSs6A6g6jmPatM4Lint4eyjW2Dlf4aswJMDYaSo5gHaQKP43BpdtlLihgdwdf9jWU/xI0ttIyuff8AOI+ulFi5JwftMywGPTuE7l1V9QwEAjXn0336GiilmRy3/bXO/rGYD9KCcV7OjCXCgLlbjDIRpv4dY3YTvpNaM/Ax93uW1GrIFJ6nIqUw7K3zIcg8xNq2RiD4lV4JgQiIw0IGed9YC/1rs/F7yMq20Zu9krG2aTnU9IIOvSp3ZfCMfARp3JHuHVY+n1rnauth0tZlJDM7qQJUBm1Ses6/6jRk5LA8xnRuVfvGYPx+J7uzcTE4bNeZswuwSgJAVAGWCsCByGp61y4tYw2GwbMJNwCV8Rln5E6668ulEuIY44sd1sh+NzoAAZOvXlG9UbtRZC3UUXO8RSTPTSAJ/F60zWjOV8ATRssVA3kmD+yOCOIxQa5spzOT6zRDtxx/v7uRD/CtbRsT1qu3McUdu7Yrm3iojtMIDq3+f3rW4AmWeTLhgsK9nh9tcx/5m73rCNMuQws89CpOomY5GVhfh0E/TUR0Ak+nX5WLjXB3vYPDXbMvbw9pbbWh8QIADN1LTpA5QRzNVu0xVoZW884IYTtqYMTyO4HU0pnPMIZ1v3+gO/OJJgDNHzAjn6VGwWPOHvpfRQ+XQoCZZToy+8aeYFSrwgDWV3Ect+R6HWI2NWLs9wYLlxGIXJZtjMrERmbSMqMJbSdhuBUyMSKCTgTSeBYuw6uLJHgaH0gglVaWHmpU1D412kVDkRgrAkMXR4031EQPME+lYxicRirmKv4nDMbdt3AIzEq4CwFddnEE8tJ0ivdvtddQ9y7XUykyrhb6KCZzKzLnUa6AaR560xWBxuhTWQ2Wmg43iYZjmTDSRMv3pJ1jTNE+mvKhuLx9pLbOzS4jQJltDpkXdjAOug330ipXu0RuHL3upBUKLZGbfVfANfrPWKl8E7PY7G3dVu2LAMq18EkmYAMhWcmSYGg+VFstVVyJx3CDA7hjsVxlMQ120NHb+Jb5ToQU8iAo+flV/uWgolhoAAOpgdKgcE+zPB2T3he5cuA6sHyAEclCQQPcnzol2i7P37tvLh8VctHmGAeRroHjMp28Un96yrNNuy47MXycczPu1JuOxd5B2ASCba8tCdWlh6TPSovCcGyZQkAMQWXZpgiVnQTpPrXLFs+HxP3TEIQ7qtsZDJcu0K6TpBJ3OxHlUpeEhLxtqxfu2Ia40ZpkFlB0lRty5+4BWyrzBLWztLRwDhl+xfDBsPlYRcDXIunU5dACCRPXXMfe2cU4pasW8991tp+ZjAPkDtPlvVGscHs5P+mAR5QfmRrzohg7mWbF0G7h7nhZH1gE6HXl+mkVb5GPtHBUUXgyncfx9vHYk3lE2kGS2SInmzQdgf2o9wayEWdhM/IUO4hwf7tiDZGqzKHqrDT35eoNEscwS2JMKolj66x51W04AQTLcksSZ6s8Ts273eXQFQxJA18iQN4rQeH3ke2r2yGRhII51hePxhuPtoNh+56Ue7GcexFi8ttQbttzHdDefzJ0I5zoY16h7TL6a4xxLVuRwZsFPXkGnpyMx6VNSqSRqVNSrs5AnG8HbQ/eBaZ7gIACzqdIJUbxFR+L4TE303W2mUMViXka5domRvNWNqq/F+L4m3YZntrbbMFBzDUGZjpt8ppa1FAJOcfT/cZqZiQBjP1/1KZZvmwxIynN4XYKVkEvIMSCBA32knlUJLkXreWcufaVMAEa6amTsfMzFFMbxC5dyEuJ0GaABAOuXTWRO/lrNC72Ndbnd5pRCB+bLMiATtIynWdz5VkP5A5mkwJQ58wbxqySWA3AkHzViu/kUobjVzW7qjRs4iOYKEg+vhC+4qzdpsGbbXTyS4R7MyXFOnlePyoNxPDqLbOpMG2jecyASCOgNXqOMD6/9TAZSDNE7O8Vt4jD2rvhL5BMbo0DMB0/pU58TG5rIMPhr8C7hrhVy4TLbPiLEnw5DuNyJFaxwLgN4WwcXcDufwKMoHkSPiPpA9d6z7Pg5sclCMfXxNajWpt+YcwVxHitm9iMNYEMTe8R5LCMQJ6kxp61fbRXJB/zWh9vhyCCtm2IMg5Vmesxv513Z4MsvuNffStOnQilAqkHjzFbmNjbsRYHCoHcr+ZvbNlY/UfWu120FULECIFQOE4buC5zZrbsX81J39RpvRK/eDKFBFM0IUJz3ABeYCuW2BJZVbXQxP0NVjtjwkYlGKKqXF2PI6ag9JkfKroWK6HaojWAwciDJj6f3pleTHWIxyJ89ngt5CQ6HQydCRHPWlY4RcYd4qyCYAkAgTBJnStktcId7rBXKkajSRrowIO/wr8qBcS4C+HuZ7d5UeCTlnLrvCmQDUvdlUERKxsQHwFcThs3dXGMiJyEGANIW4PHAEBhy5iiVvtRafL97s98wESWWYnWCx+gJGtV/AcQv3bjfHdRd2dyoEkw3xDoesVK4hw0sVvOFzAknTwOOR0OXOIBJEggTSZYhvnllNi4H8y34XjeAs289jD2+8kwG8bA9ST8Oo61VONdoL+KZ1zl2gy34EXnA2HSarmLuZrjAkoSJGsA+pG39vOufDsYwHd25lo0HxMZ0FHRM8sZo6cqpYWDBHck8M446WzbBy9Ry6GpPA8dbd2d3h28KknkBpA2IkwfKh1ngzveFkhs2YyIMFug6jzqz3OE2rbBMSVFsqBb0KQealhtHKdDNWuPyZGf0g7WtarenQ/M69lMCTjLeaFWy+eT1jRF5mcwM9JNbcHR1gn0/YVhmNxS2mm1mYgKAwlhpIBZta1vC3xesqbeisoJ6g8xrWe1rsNx6x17xD1mtJLQvhMZbshldiNdBBJ+gmKJ2mDDMDIOxqtfdVtjMTmdtsxk+v00otwMEWRpzPPz/rRdLc+7Yw4xx/M6p8T1j8FZbK1xEY2zmRmAJUjmp3FZ8LeR3B0bMZJHWZ/f6VeON4lYCg6HfWNvM6AVVrWItY2TZKrdHxWmMFhsHUjfTl+m5JcDYSB4hqm2nmeBeiNYPmeXt7ddqfKzlSDOYxE7a66f5EmuX/D8TIAtPIMHSV9fENf29KKMlnCKL2IIzxFq0IzkxqAAddeewHSgJUc88CGewAQX2yZRiUk/BbXN83P6VVuLYw3m00QbD9/WuXEcbcvXWZtS7Zm6dAo8gAAPSpFjBFnVRqWMADmfL+td9RC5f8TIYFm4kLAcNa4wRV1P+STyFah2V4DbwyyIa43xP/8AVeg/WmwHZ1LKqwds/wCKIynyIP6iDRqwtP0Zblu4datnJksUq8rXqmJaPTUqVSdjUqVKuzkVQuMR3RJtC7EQhEieux/SptMa4wyMToODmZ/2uNp0ChFW6IN0AaRAkFgNtYg7+1UvEuQbaaSxB1mQJJGoPiiNiedbBxHg6Ol0Kqh7sZiRMxET7f1rLuI8MNq5BMw5PhMkkDw7gcwwGnPXlWXqKipBY9zSosDKQPEK8U8a3AOSi6PMKTP/ANPnQXiGFb7tnykoZX18e3n8H1ow1vxCZm2WtnXdWjQ/6SPlpRLg9hb1rE4QjVSHt+QbVYPkwI9xWXQRu2jsczuu05NZf3IP/HMhfZLwfW/iGAiQijcZgCzMPZwPc1oooL9ndjJhChEMtx1b1ETRZDW6vCD6zPrGBOopmApg1MXqphZxZQDsIOhFV3jPDHtnPbJy9OgqzMa94izm0OoIgii1/MCDKsdrAiVnB49tn+vOpmDu5HJjwkCQP19a54nCd2ddV6n969WCM0DWRtvVhxCsdwzC13Dr8agEiCCBqRI086rnbLCB7BVAGuOwW3z/ABajfY7f5NGcHjDbOR1YCfCYMehPKieHS0YcKo/IYE9DHTppRP8AIRVhgzIftH7NmxYttZthAha5dWcwti5lGRW/EM6n0mqbhGyWrhFwZnEQZgdYHnMfWt14n3N83UaWHwEHbQESOW5b/IrG24EyXGtEA5iQp/lGgbymg3bQMnqanw9w4NbLu54GcY+siWUsgJbuqHDzLDkeU8xGmx+dG+D4FLN49zaRSsw9xiSJAHhGh/Dv5nrQ3ifADh2RyxYDUjQ6R5CpCYhWbMCRC8tzqP2ms6xi3+BOPPff2j9mjo3l3BwT1nPfmN2xxCZ0KgIADqdJZoJiPhiPrQLE8Qa4qqzFo+EHUz0B50b4niUNy33igqQYzbHYfvXfhfC7aI2Iy5w5K2LX4GZfjuPqPADAyyJIadoLGnsxWARjiA1VQ06EVkFc4xOnZrChcOTtmLF49121I8IGlWP7MuLELcssSy2gCCddDMKfMZfkwqpY3EXmibrDQFQngAXUCFSAs9BtFcLXE7tvS89x7ZjxAkXVgkgq3xSD+FpU7RQkrG5iT2ZghCCTNfv4+TOrA7GT+nKjPDcQe7GgAEyT6nlWKXe0fEUvmyptvlAYXCshkYAo3Lcfv0q4WO0N0WpNly/TNCz6xMT5Vwn0W+YjP3jVOkscblXMndvsWlvCXPGVe5KoBpMkTA5aTr09azvg3D3ueKXAX8YkwfUbV57Wfe2cX75BX4VyA5UnWI5f+RmeZ2ofw3ijoQyOR6GrKcgEHI+k9H8O09VdZVwCx9wDiXc8axFpQv3t7kmAMxJ/9v71J7N9n7mId8RdLEnwqWJJPUyeU6e1VtOMPedMyKSCB4FgkmYn5b1rXBw1nDL3p8QWT5c49hp7VxyWUqScGZ3xiutWVVUA45x9+OBKnxThaWNTqx2H7+lSuxnBXTNibxlmnJOmVeQA5aV1wmF+8XjcfS2p19tQn7n1qVi+NF2yWllF0LbLpyHX1pGnanJOB4zF10gQBVGT5+n0hq2Szb6CiSCh3B7odJG4MH18vnRMCt6jaUBXkGIWhg5DdiehT01KjwUelSpq5JFSpU1dkj0qalUkj0C4p2bs3JaCr6kMDz6kHQ0cry40odiK4wwzLo7KcqZn/wDwxjnuj4SYZfRdx5iamdk+GFsQb+aCi92y/mGpM9PwEe9WDhtgd2w6s36xQnhDnD4l7Z+FxIPKViPmDHtWMmlCNW48jB+81LdQbK7E9jkfbzLTg7S23IAjOc3q0a/QfSuOMslWJ5H9elccViQwidRqCOVd8HxZHGS7AO0/hP8AStUDI2/iZYbBkbPT9586JHh6HVSY8jIrm+DtWwWdoUCSWIUAcyTVfSaX3iccLZzHyrMsb9pJXjJtK4OEGXDttGeTN0Hydsp5QJ6UvtC+05ShwuAacwh740AGxFrrP59uk7jIrODBB8QEKTBOhAjQHmTrp5UZE2iUZsmfVdi/bcfhYbHmPSn+9Wk8C5QegEAesaVhHZn7QsRhYtXrZvLoNZF0aSN9HMEbweprRF4h39o3cMyOzCQD4YJH4huvWKvK5hXjuIJdLSXsz3DlyiAFG7uSByXkebCp+KZbaF2PwjQA9NAB/nOq9gMJkYMxzXWGXMdyNzC/gWeXpMmuuNRroyqdAZnqR0nlUnMznaFhZeGOuuZjBkGRE+dUjjfF1bEF1iJKDYDw7RHSY/01c7vCr2QrmGUjUwJiNYjyqkYfhVm4hzKFMHJ5HkJ6UjrWRV2sO5r/AAipjYXXHHf6yHxriKqpdjMjUHUGZHOq9wnCyQ4NxAdQsjkdSDuR5R7mpfE+FtadNc0tABM+YEtMD0r0mGuqwdhmA0I3jUz6gaD2pevalfynuGt32arBUgc+f7xLA+Fw922QVPeDdiZnpvqDFd8ZaVbWFbXuhZZTB597dzmCYMGdxpmB0oKOMWQp1AOugBzTPMcvejXZm/bvZsNcZ1XMbtm8dAjMozow5W20PkalKPhgevE58RtqNarxuB5x0BIxCmZGYNtJG+6t5mFYkFt+lR8RgwwgCRvJ1cAxpB156bba0YxPZTGqZ7oOI0yQyMJGoy6ajqP1qfw3sXcaGxI7u0IOQmXMckCnwgiAZ10q61PnqZG4SNgMIqW7BIg/d0EnfLmfIDqdY8zvUW7xgozKVlbZGdhGgMHY6nlVr4hbYuXUQNBlHIDQAegoO+H7zMGIABkDqRBg9R5Gs3VqUuZrF4PU3dPY5oVaWGR3n/qNxFlvWLitoChk7Rpv7bz5Vmt3hb22Atp4TsZzZvPQworQu0uOVMO40DuCq8zJ0nzA3oVchhZ1ABC+LkJ0JruisetcjomIfFrtlqheCBDf2Y8FYzeuiCDlUR05nrvVo49jO8uCwhAjW43JR5+flzMedQ0wr4BWd7qZXGVdD8W4MeQk+dDsPi0APdKXYmSx3J6kmi6i1gNpXk/tL6Kt7j6thyR1n++ITxZBUW1lLK8vxP1ny/XnUC5ii3gtLIGhjRR6n9q9pgrtzVtug0HudzU+zwm4YEgDoBSo02otOduf+BNE3UVDBYZ/MM9m8Usd0AAQJ0Mz1MxvRyhXBuGrakxqeZ3oqK9FpkdKwHxn6Tz+pdHsLJnH1j0qalTEBHpU1KpJFSpqVSSPSpqVSSPTGlSqSSNh7eWR5k/Oo+OwYcgncGR8oI+VT2WmIqgQYx4lixzmDPutc2w9FCtc2t1eUxByqRtIrjjsI11GTOyE6ZgAxHs4IPTUc6Kd1XtbVSdmN9rew9xLr3UCKjZ2QLncltDB8MIWLHnAj2qHjuyVzDZTea1bVlLrni46hUPeW8uXIWYsIjmBqIM7oFoL2o7OjFKo7woVzRpmUhvAQVkfMEMATBEmpJMVwnDTf/h2hN4oXOWFUqtm3/Dy5cxu94snYHU/iEaLwzuVwdi7bt9zcyDKO7Ny6yIQrgKhDXARB8swMaVaMLwG2rK5X4Gc2iHcnLcAzBwTqJkBTIACxEaTcJw63bVQiABAQvMgEyQCdQNtPIVJJBfCBt15R7dOoB51MsYWKmBK9hakki37fhrN8b2avLcPdkFJ0EwR5a1p9xahXMICaDbSlow0Z02qs05JQ9zMOJ8AZct1/FlOo3ABG/mZivOFvWkUhVkn8RMwDyE7Vpd/CiNqr3EOy63B/CItMTqQskjoJMKfODSduizjYcDHU06Pi+AfVXJ+koVnhNlrzyf4jEBVjcmNjqG3GkVbX7EsML3aZVusyljJygAwYgTqu4rl2O4M7Ygi+Acvj1gnNOVRI/KynXTYVpHd0XT1ZBJ+0xLbPVdnxjJJlM7PdnWw2vePMRlV2W3v8WTNExH160fXDk6miXc04t02qhRgSoED4jByKoF/s3esC/dDMIDNmLSznkSB08xyFas1uuLWQeVUtpWwYM7kg5BmE3OHm6SQWZ2GksTtqRr5TRLsrhbTXFW60oRmWSQsg8zoCDWojs3h0D93bCM4glSZjmBPw+1VgdkMSMUhBHdBhqCAAkyVK7kxpzpBqLK8cZg2Bzk8y0cRwLYjISQMu0DTWNfkK7YHgaJy1ouiQK6RTwoTdvxzGvXs2bN3EjJhVHKuotCulKjQMYCvVNSqSR6VNSqSR6alSqST/9k=",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 25,
                UserId = user.Id
            });
            recipes[0].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[0],
                    ProductId = products[0].Id,
                    Quantity = 4
                },
                new ProductRecipe
                {
                    Recipe = recipes[0],
                    ProductId = products[1].Id,
                    Quantity = 10
                },
                new ProductRecipe
                {
                    Recipe = recipes[0],
                    ProductId = products[2].Id,
                    Quantity = 13
                }
            };
            
            recipes.Add(new Recipe
            {
                Name = "BALSAMIC CHICKEN CAPRESE",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Easy,
                Photo = "data:image/jpeg;base64,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",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 50,
                UserId = user.Id
            });
            recipes[1].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[1],
                    ProductId = products[3].Id,
                    Quantity = 2
                },
                new ProductRecipe
                {
                    Recipe = recipes[1],
                    ProductId = products[5].Id,
                    Quantity = 13
                },
                new ProductRecipe
                {
                    Recipe = recipes[1],
                    ProductId = products[2].Id,
                    Quantity = 3
                }
            };
            
            recipes.Add(new Recipe
            {
                Name = "MEDITERRANEAN CHICKEN SALAD",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Easy,
                Photo = "data:image/jpeg;base64,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",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 15,
                UserId = user.Id
            });
            recipes[2].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[2],
                    ProductId = products[7].Id,
                    Quantity = 8
                },
                new ProductRecipe
                {
                    Recipe = recipes[2],
                    ProductId = products[11].Id,
                    Quantity = 10
                },
                new ProductRecipe
                {
                    Recipe = recipes[2],
                    ProductId = products[16].Id,
                    Quantity = 30
                },
                new ProductRecipe
                {
                    Recipe = recipes[2],
                    ProductId = products[18].Id,
                    Quantity = 30
                }
            };
            
            recipes.Add(new Recipe
            {
                Name = "EASY HOMEMADE BEEF STEW",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Medium,
                Photo = "data:image/jpeg;base64,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",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 44,
                UserId = user.Id
            });
            recipes[3].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[3],
                    ProductId = products[1].Id,
                    Quantity = 8
                },
                new ProductRecipe
                {
                    Recipe = recipes[3],
                    ProductId = products[2].Id,
                    Quantity = 1
                },
                new ProductRecipe
                {
                    Recipe = recipes[3],
                    ProductId = products[5].Id,
                    Quantity = 3
                },
                new ProductRecipe
                {
                    Recipe = recipes[3],
                    ProductId = products[18].Id,
                    Quantity = 3
                }
            };
            
            recipes.Add(new Recipe
            {
                Name = "HEALTHY OATMEAL RAISIN COOKIES",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Medium,
                Photo = "data:image/jpeg;base64,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",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 35,
                UserId = user.Id
            });
            recipes[4].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[4],
                    ProductId = products[0].Id,
                    Quantity = 2
                },
                new ProductRecipe
                {
                    Recipe = recipes[4],
                    ProductId = products[12].Id,
                    Quantity = 13
                },
                new ProductRecipe
                {
                    Recipe = recipes[4],
                    ProductId = products[11].Id,
                    Quantity = 3
                }
            };
            
            recipes.Add(new Recipe
            {
                Name = "HEALTHY SHRIMP SALAD",
                Description = "Preheat the oven to 400ºF and line a baking sheet with parchment paper. Place the cauliflower florets into a large bowl.Drizzle with olive oil, and sprinkle with all the seasonings. Using your clean hands toss well to get all pieces evenly coated. Transfer the cauliflower to the prepared baking sheet and arrange in a single layer. Roast for 15 minutes, then sprinkle with the parmesan cheese. Bake for 15 minutes more, or until crisp tender. Garnish with freshly chopped parsley and enjoy!",
                Difficulty = Difficulty.Hard,
                Photo = "data:image/jpeg;base64,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",
                NotesAndTips = "Cut the cauliflower into bite-size pieces. To keep this roasted cauliflower recipe vegan friendly, you may use dairy-free cheese or skip the cheese completely.Use garlic powder, not fresh garlicAny other oil of choice will work.",
                TimeToFinish = 45,
                UserId = user.Id
            });
            recipes[5].ProductsQuantities = new List<ProductRecipe>
            {
                new ProductRecipe
                {
                    Recipe = recipes[5],
                    ProductId = products[9].Id,
                    Quantity = 2
                },
                new ProductRecipe
                {
                    Recipe = recipes[5],
                    ProductId = products[4].Id,
                    Quantity = 13
                },
                new ProductRecipe
                {
                    Recipe = recipes[5],
                    ProductId = products[17].Id,
                    Quantity = 3
                }
            };

            return recipes;
        }

        private static IEnumerable<Workout> GetWorkoutsData(User user, List<Exercise> exercises)
        {
            var workouts = new List<Workout>();
            workouts.Add(new Workout
            {
                Name = "The 'Get Peeled'",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Easy,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 20,
                CaloriesBurned = 150,
                UserId = user.Id
            });
            workouts[0].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[0],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[0],
                    ExerciseId = exercises[1].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[0],
                    ExerciseId = exercises[2].Id,
                    Sets = 2,
                    Repetitions = 18
                },
            };
            
            workouts.Add(new Workout
            {
                Name = "40 Minute Blast",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Hard,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 45,
                CaloriesBurned = 200,
                UserId = user.Id
            });
            workouts[1].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[1],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[1],
                    ExerciseId = exercises[1].Id,
                    Sets = 2,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[1],
                    ExerciseId = exercises[3].Id,
                    Sets = 3,
                    Repetitions = 18
                },
            };
            
            workouts.Add(new Workout
            {
                Name = "The Kettlebell Conditioner",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Medium,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 25,
                CaloriesBurned = 180,
                UserId = user.Id
            });
            workouts[2].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[2],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[2],
                    ExerciseId = exercises[2].Id,
                    Sets = 4,
                    Repetitions = 10
                }
            };
            
            workouts.Add(new Workout
            {
                Name = "The Kettlebell Conditioner 2",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Easy,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 15,
                CaloriesBurned = 180,
                UserId = user.Id
            });
            workouts[3].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[3],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[3],
                    ExerciseId = exercises[1].Id,
                    Sets = 4,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[3],
                    ExerciseId = exercises[2].Id,
                    Sets = 2,
                    Repetitions = 10
                }
            };
            
            workouts.Add(new Workout
            {
                Name = "The Kettlebell Conditioner 3",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Medium,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 15,
                CaloriesBurned = 180,
                UserId = user.Id
            });
            workouts[4].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[4],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[4],
                    ExerciseId = exercises[1].Id,
                    Sets = 4,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[4],
                    ExerciseId = exercises[2].Id,
                    Sets = 2,
                    Repetitions = 10
                }
            };
            
            workouts.Add(new Workout
            {
                Name = "The Kettlebell Conditioner 4",
                Description = "Strap on your game face, stretch, get warmed up and let's roll! This is cardio and abs workout that you can use to break the gym boredom. It also involves resistance training. If you are feeling like you need a break from the same old dull workout routine, but don't want to train like a wet noodle either, give this a try. You will cycle between a resistance exercise, cardio and an abdomimal exercise. Rest 15-30 seconds between each step of the workout. It is best to use 2 sets of dumbbells for this workout, a heavier set for the tougher lifts, and a lighter set for isolation movements. You can also set up near a dumbbell rack and grab dumbbells as you need them, if possible.",
                Difficulty = Difficulty.Hard,
                Photo = "https://img.etimg.com/photo/msid-74747053,quality-100/for-miles-a-great-bodyweight-workout-would-include-squats-push-ups-walking-lunges-.jpg",
                TimeToFinish = 14,
                CaloriesBurned = 180,
                UserId = user.Id
            });
            workouts[5].WorkoutsExcercises = new List<WorkoutExercise>
            {
                new WorkoutExercise
                {
                    Workout = workouts[5],
                    ExerciseId = exercises[0].Id,
                    Sets = 3,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[5],
                    ExerciseId = exercises[1].Id,
                    Sets = 4,
                    Repetitions = 10
                },
                new WorkoutExercise
                {
                    Workout = workouts[5],
                    ExerciseId = exercises[2].Id,
                    Sets = 2,
                    Repetitions = 10
                }
            };

            return workouts;
        }
    }

}
