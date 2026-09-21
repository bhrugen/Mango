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


        [HttpPost]
        public async Task<IActionResult> CouponCreate(CouponDto model)
        {
            if (ModelState.IsValid)
            {

                ResponseDto? responseDto = await _couponService.CreateCouponsAsync(model);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Handle the successful response
                    TempData["success"] = "Coupon created successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.ErrorMessage;
                }
            }
            return View(model);
        }

        public async Task<IActionResult> CouponDelete(int couponId)
        {
            ResponseDto? responseDto = await _couponService.GetCouponByIdAsync(couponId);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                CouponDto? model= responseDto.GetResult<CouponDto>();
                return View(model);
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
            }

            return NotFound();
        }

        [HttpPost]
        public async Task<IActionResult> CouponDelete(CouponDto couponDto)
        {
                ResponseDto? responseDto = await _couponService.DeleteCouponsAsync(couponDto.CouponId);
                if (responseDto != null && responseDto.IsSuccess)
                {
                    // Handle the successful response
                    TempData["success"] = "Coupon deleted successfully";
                    return RedirectToAction(nameof(CouponIndex));
                }
                else
                {
                    TempData["error"] = responseDto?.ErrorMessage;
                }
            return View(couponDto);
        }
    }
}
