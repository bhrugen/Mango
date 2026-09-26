using Mango.Web.Models;
using Mango.Web.Service;
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

        public async Task<IActionResult> Remove(int cartDetailsId)
        {
            ResponseDto? responseDto = await _cartService.RemoveFromCartAsync(cartDetailsId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Item has been removed from the Shopping Cart";
            }
            return RedirectToAction(nameof(CartIndex));
        }

        public async Task<IActionResult> UpdateCount(int cartDetailsId, int change, int productId)
        {
            var UserId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault()?.Value;

            CartDto cartDto = new()
            {
                CartHeader = new CartHeaderDto()
                {
                    UserId = UserId,
                },
                CartDetails = new List<CartDetailsDto>()
                {
                    new CartDetailsDto()
                    {
                        Count = change,
                        ProductId = productId,
                        CartDetailsId = cartDetailsId
                    }
                }
            };
            await _cartService.UpsertCartAsync(cartDto);
            return RedirectToAction(nameof(CartIndex));
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
