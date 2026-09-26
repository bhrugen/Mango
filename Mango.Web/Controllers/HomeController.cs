using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly IProductService _productService;
        private readonly ICartService _cartService;
        public HomeController(IProductService productService, ICartService cartService)
        {
            _productService = productService;
            _cartService = cartService;
        }
        public async Task<IActionResult> Index()
        {
            ResponseDto? responseDto = await _productService.GetAllProductsAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                List<ProductDto>? products = JsonConvert.DeserializeObject<List<ProductDto>>(Convert.ToString(responseDto.Result));
                return View(products);
            }
            return View(new List<ProductDto>());
        }

        public async Task<IActionResult> Details(int productId)
        {
            ResponseDto? responseDto = await _productService.GetProductByIdAsync(productId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                ProductDto? product = JsonConvert.DeserializeObject<ProductDto>(Convert.ToString(responseDto.Result));
                return View(product);
            }
            return View(new ProductDto());
        }

        [HttpPost]
        [Authorize]

        public async Task<IActionResult> Details(ProductDto productDto)
        {
            CartDto cartDto = new CartDto
            {
                CartHeader = new CartHeaderDto
                {
                    UserId = User.Claims.Where(u=>u.Type=="sub").FirstOrDefault()?.Value
                }
            };  

            CartDetailsDto cartDetailsDto = new CartDetailsDto
            {
                Count = productDto.Count,
                ProductId = productDto.ProductId
            };

            cartDto.CartDetails = new List<CartDetailsDto> { cartDetailsDto };

            ResponseDto? responseDto = await _cartService.UpsertCartAsync(cartDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                TempData["success"] = "Item has been added to the Shopping Cart";
                return RedirectToAction("Index");
            }
            return View(productDto);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
