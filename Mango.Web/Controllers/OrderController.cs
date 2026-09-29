using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    public class OrderController : Controller
    {
        private readonly IOrderService _orderService;
        public OrderController(IOrderService orderService)
        {
            _orderService = orderService;
        }
        public IActionResult OrderIndex()
        {
            return View();
        }


        [HttpGet]
        public async Task<IActionResult> GetAllOrders()
        {
            string? userId = User.IsInRole(SD.RoleAdmin)
               ? null
               : User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;

            var response = await _orderService.GetAllOrder(userId);
            if (response != null && response.IsSuccess)
            {
                var result = Convert.ToString(response.Result);
                var orders = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(result);
                return Json(new { data = orders });
            }
            return Json(new { data = new List<OrderHeaderDto>() });
        }
    }
}
