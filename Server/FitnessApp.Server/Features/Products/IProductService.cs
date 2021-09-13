namespace FitnessApp.Server.Features.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Products.Models;
    using FitnessApp.Server.Infrastructure.Services;

    public interface IProductService
    {
        Task<IEnumerable<ProductListingModel>> AllProducts();
        
        Task<IEnumerable<ProductsByNamesModel>> AllProductsByNames();

        Task<int> Create(CreateProductRequestModel model);

        Task<Result> Delete(int id);

        Task<ProductDetailsModel> Details(int id);

        Task<Result> Update(int id, UpdateProductRequestModel model);
    }
}
