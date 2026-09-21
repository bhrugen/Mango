using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class CouponController : Controller
    {
        private readonly ICouponService _couponService;
        public CouponController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> CouponIndex()
        {
            List<CouponDto>? coupons = new();
            ResponseDto? responseDto = await _couponService.GetAllCouponsAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                coupons = responseDto.GetResult<List<CouponDto>>();
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
            }
            
            return View(coupons);
        }

        public async Task<IActionResult> CouponCreate()
        {
            
            return View();
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            
            return View();
        }
    }
}
