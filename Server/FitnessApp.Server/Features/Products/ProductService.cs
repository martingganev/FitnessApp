namespace FitnessApp.Server.Features.Products
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using FitnessApp.Server.Data;
    using FitnessApp.Server.Data.Models.Eating;
    using FitnessApp.Server.Features.Products.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.EntityFrameworkCore;

    public class ProductService : IProductService
    {
        private readonly FitnessAppDbContext context;
        private readonly ICurrentUserService currentUser;

        public ProductService(FitnessAppDbContext context, ICurrentUserService currentUser)
        {
            this.context = context;
            this.currentUser = currentUser;
        }

        public async Task<IEnumerable<ProductListingModel>> AllProducts()
            => await context
                .Products
                .Select(p => new ProductListingModel
                {
                    Id = p.Id,
                    Name = p.Name,
                    Fats = p.Fats,
                    Protein = p.Protein,
                    Sodium = p.Sodium,
                    Sugar = p.Sugar
                })
                .ToListAsync();

        public async Task<IEnumerable<ProductsByNamesModel>> AllProductsByNames()
            => await context
                .Products
                .Select(p => new ProductsByNamesModel
                {
                    Id = p.Id,
                    Name = p.Name
                })
                .ToListAsync();

        public async Task<int> Create(CreateProductRequestModel model)
        {
            var product = new Product
            {
                Carbs = model.Carbs,
                Name = model.Name,
                Fats = model.Fats,
                Protein = model.Protein,
                Sodium = model.Sodium,
                Sugar = model.Sugar
            };

            this.context.Add(product);

            await this.context.SaveChangesAsync();

            return product.Id;
        }

        public async Task<Result> Delete(int id)
        {
            var product = await this.context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if(product == null)
            {
                return "Product Not Found.";
            }

            this.context.Products.Remove(product);

            await this.context.SaveChangesAsync();

            return true;
        }

        public async Task<ProductDetailsModel> Details(int id)
            => await this.context
                .Products
                .Where(p => p.Id == id)
                .Select(p => new ProductDetailsModel
                {
                    Id = p.Id,
                    Carbs = p.Carbs,
                    Name = p.Name,
                    Fats = p.Fats,
                    Protein = p.Protein,
                    Sodium = p.Sodium,
                    Sugar = p.Sugar
                })
                .FirstOrDefaultAsync();

        public async Task<Result> Update(int id, UpdateProductRequestModel model)
        {
            var product = await this.context.Products.FirstOrDefaultAsync(p => p.Id == id);
            if (product == null)
            {
                return "Product Not Found.";
            }

            product.Carbs = model.Carbs;
            product.Name = model.Name;
            product.Fats = model.Fats;
            product.Protein = model.Protein;
            product.Sodium = model.Sodium;
            product.Sugar = model.Sugar;

            await this.context.SaveChangesAsync();

            return true;
        }
    }
}
