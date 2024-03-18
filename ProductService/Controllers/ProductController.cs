using Microsoft.AspNetCore.Mvc;
using ProductService.Models;
using ProductService.Services;
using SharedLibrary.Interfaces;

namespace ProductService.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ProductController : ControllerBase
    {
        private readonly IDBService<Product> _productService;

        public ProductController(IDBService<Product> productService)
        {
            _productService = productService;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllProducts(){
            var products = await _productService.GetAllAsync();
            if (!products.Any())
                return NotFound();
            return Ok(products);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetProductById(int id){
            var product = await _productService.GetByIdAsync(id);
            if (product == null)
                return NotFound();
            return Ok(product);
        }

        [HttpPost]
        public async Task<IActionResult> CreateProduct(Product product){
            var productId = (await _productService.CreateAsync(product)).ProductId;
            product.ProductId = productId;
            var actionName = nameof(GetProductById);
            var routeValues = new { id = productId };
            return CreatedAtAction(actionName, routeValues, product);
        }

        [HttpPut]
        public async Task<IActionResult> UpdateProduct(Product product){
            var updatedProduct = await _productService.UpdateAsync(product);
            return Ok(updatedProduct);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteProduct(int id){
            var deletedProduct = await _productService.DeleteAsync(id);
            return Ok(deletedProduct);
        }
    }
}