using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class ProductController : Controller
    {
        private readonly IProductService _productService;
        public ProductController(IProductService productService)
        {
            _productService = productService;
        }
        public async Task<IActionResult> ProductIndex()
        {
            List<ProductDto>? products = new();
            ResponseDto? responseDto = await _productService.GetAllProductsAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                products = responseDto.GetResult<List<ProductDto>>();
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
            }
            
            return View(products);
        }

        public async Task<IActionResult> ProductCreate()
        {
            return View();
        }


        [HttpPost]
        public async Task<IActionResult> ProductCreate(ProductDto model)
        {
            if (ModelState.IsValid)
            {

                ResponseDto? responseDto = await _productService.CreateProductsAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Handle the successful response
                    TempData["success"] = "Product created successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.ErrorMessage;
                }
            }
            return View(model);
        }


        public async Task<IActionResult> ProductEdit(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                ProductDto? model = responseDto.GetResult<ProductDto>();
                return View(model);
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
            }
            return NotFound();
        }


        [HttpPost]
        public async Task<IActionResult> ProductEdit(ProductDto model)
        {
            if (ModelState.IsValid)
            {

                ResponseDto? responseDto = await _productService.UpdateProductsAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Handle the successful response
                    TempData["success"] = "Product updated successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.ErrorMessage;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> ProductDelete(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                ProductDto? model= responseDto.GetResult<ProductDto>();
                return View(model);
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
                ResponseDto? responseDto = await _productService.DeleteProductsAsync(productDto.ProductId);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Handle the successful response
                    TempData["success"] = "Product deleted successfully";
                    return RedirectToAction(nameof(ProductIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.ErrorMessage;
                }
            return View(productDto);
        }
    }
}
