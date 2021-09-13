namespace FitnessApp.Server.Features.Products
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using FitnessApp.Server.Features.Products.Models;
    using FitnessApp.Server.Infrastructure.Services;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;

    using static Infrastructure.GlobalConstants;

    [AllowAnonymous]
    public class ProductsController : ApiController
    {
        private readonly IProductService products;
        private readonly ICurrentUserService currentUser;

        public ProductsController(
            IProductService products,
            ICurrentUserService currentUser)
        {
            this.products = products;
            this.currentUser = currentUser;
        }

        [HttpGet]
        [Route(nameof(AllProducts))]
        public async Task<IEnumerable<ProductListingModel>> AllProducts()
            => await this.products.AllProducts();
        
        [HttpGet]
        [Route(nameof(AllProductsByNames))]
        public async Task<IEnumerable<ProductsByNamesModel>> AllProductsByNames()
            => await this.products.AllProductsByNames();

        [HttpPost]
        [Route(nameof(Create))]
        [Authorize(Roles = AdminRole)]
        public async Task<ActionResult> Create(CreateProductRequestModel model)
        {
            var id = await this.products.Create(model);

            return Created(nameof(this.Create), id);
        }

        [HttpGet]
        [Route(Id)]
        public async Task<ProductDetailsModel> Details(int id)
            => await this.products.Details(id);

        [HttpPut]
        [Authorize(Roles = AdminRole)]
        [Route(Id)]
        public async Task<ActionResult> Update(int id, UpdateProductRequestModel model)
        {

            var result = await this.products.Update(id, model);

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
            var result = await this.products.Delete(id);
            if (result.Failure)
            {
                return BadRequest();
            }

            return Ok();
        }
    }
}