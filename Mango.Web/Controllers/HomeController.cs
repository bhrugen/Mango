using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System.Diagnostics;

namespace Mango.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ICouponService _couponService;
        public HomeController(ICouponService couponService)
        {
            _couponService = couponService;
        }
        public async Task<IActionResult> Index()
        {
            ResponseDto? responseDto = await _couponService.GetAllCouponsAsync();
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                List<CouponDto>? coupons = JsonConvert.DeserializeObject<List<CouponDto>>(Convert.ToString(responseDto.Result));
            }
            return View();
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
