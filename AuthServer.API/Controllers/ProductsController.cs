using AuthServer.Core.DTOs;
using AuthServer.Core.Entities;
using AuthServer.Core.Services;
using AuthServer.Service.Mappings;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace AuthServer.API.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class ProductsController : CustomBaseController
    {
        private readonly IGenericService<Product, ProductDto> _productService;

        public ProductsController(IGenericService<Product, ProductDto> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetProducts()
        {
            return ActionResultInstance(await _productService.GetAllAsync());
        }
        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(string id)
        {
            return ActionResultInstance(await _productService.GetByIdAsync(id));
        }
        [HttpPost]
        public async Task<IActionResult> SaveProduct(CreateProductDto dto)
        {
            return ActionResultInstance(await _productService.AddAsync(ObjectMapper.Mapper.Map<ProductDto>(dto)));
        }
        [HttpPut]
        public async Task<IActionResult> UpdateProduct(ProductDto dto)
        {
            return ActionResultInstance(await _productService.UpdateAsync(dto, dto.Id));
        }
        [HttpDelete("{id}")]
        public async Task<IActionResult> RemoveProduct(string id)
        {
            return ActionResultInstance(await _productService.RemoveAsync(id));
        }
    }
}
