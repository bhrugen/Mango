using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

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
                ResponseDto? responseDto = await _productService.CreateProductAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Dish created successfully";
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
                ResponseDto? responseDto = await _productService.UpdateProductAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    TempData["success"] = "Dish updated successfully";
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
        public async Task<IActionResult> ProductDelete(ProductDto productDto)
        {
            ResponseDto? responseDto = await _productService.DeleteProductAsync(productDto.ProductId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Dish deleted successfully";
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
