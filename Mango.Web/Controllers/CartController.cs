using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        public CartController(ICartService cartService)
        {
            _cartService = cartService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartByLoggedInUser());
        }
        
        private async Task<CartDto> LoadCartByLoggedInUser()
        {
            var UserId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault()?.Value;
            if(string.IsNullOrEmpty(UserId))
            {
                return new CartDto();
            }
            ResponseDto? responseDto = await _cartService.GetCartByUserIdAsync(UserId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                CartDto cartDto = JsonConvert.DeserializeObject<CartDto>(Convert.ToString(responseDto.Result));
                return cartDto;
            }

            return new CartDto();
        }
    }
}
