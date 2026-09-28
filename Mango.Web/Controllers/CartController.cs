using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.IdentityModel.Tokens.Jwt;

namespace Mango.Web.Controllers
{
    [Authorize]
    public class CartController : Controller
    {
        private readonly ICartService _cartService;
        private readonly IOrderService _orderService;
        public CartController(ICartService cartService, IOrderService orderService)
        {
            _cartService = cartService;
            _orderService = orderService;
        }
        public async Task<IActionResult> CartIndex()
        {
            return View(await LoadCartByLoggedInUser());
        }
        public async Task<IActionResult> Confirmation(int orderId)
        {
            ResponseDto? response = await _orderService.ConfirmOrder(orderId);
            if (response != null && response.IsSuccess)
            {
                OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
                if (orderHeaderDto.Status == SD.Status_Approved)
                {
                    return View(orderId);
                }
            }
            //redirect to some error page
            return View(0);
        }

        [HttpPost]
        [ActionName("Checkout")]
        public async Task<IActionResult> Checkout(CartDto cartDto)
        {
            CartDto cart = await LoadCartByLoggedInUser();
            cart.Name = cartDto.Name;
            cart.Phone = cartDto.Phone;
            cart.Email = cartDto.Email;

            ResponseDto? response =  await _orderService.CreateOrder(cart);
            OrderHeaderDto orderHeaderDto = JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));

            if(response != null && response.IsSuccess)
            {
                return RedirectToAction(nameof(Confirmation), new { orderId = orderHeaderDto.OrderHeaderId });
            }

            return View();
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

        [HttpPost]
        public async Task<IActionResult> ApplyCoupon(CartDto cartDto)
        {
            cartDto.CartHeader.UserId = User.Claims.Where(u => u.Type == "sub").FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.ApplyCouponAsync(cartDto);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Cart updated successfully";
            }
            return RedirectToAction(nameof(CartIndex));
        }


        [HttpPost]
        public async Task<IActionResult> EmailCart()
        {
            CartDto cart = await LoadCartByLoggedInUser();
            cart.Email = User.Claims.Where(u => u.Type == "name").FirstOrDefault()?.Value;
            ResponseDto? responseDto = await _cartService.EmailCartRequestAsync(cart);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Email will be processed and sent shortly.";
            }
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
