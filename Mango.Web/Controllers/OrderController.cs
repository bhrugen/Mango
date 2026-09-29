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

        public async Task<IActionResult> OrderDetail(int orderId)
        {
            ResponseDto responseDto = await _orderService.GetOrderById(orderId);
            if(responseDto!=null && responseDto.IsSuccess)
            {
                var result = Convert.ToString(responseDto.Result);
                var orderHeader = JsonConvert.DeserializeObject<OrderHeaderDto>(result);
                if (orderHeader != null)
                {
                    return View(orderHeader);
                }
                TempData["error"] = "Order not found.";
            }
            return View(nameof(OrderIndex));
        }

        [HttpPost]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            ResponseDto? responseDto = await _orderService.UpdateOrderStatus(orderId,SD.Status_ReadyForPickup);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new { orderId });
            }
            TempData["error"] = "Error encountered!";
            return RedirectToAction(nameof(OrderDetail), new { orderId });
        }
        [HttpPost]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            ResponseDto? responseDto = await _orderService.UpdateOrderStatus(orderId, SD.Status_Completed);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new {  orderId });
            }
            TempData["error"] = "Error encountered!";
            return RedirectToAction(nameof(OrderDetail), new {  orderId });
        }

        [HttpPost]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            ResponseDto? responseDto = await _orderService.UpdateOrderStatus(orderId, SD.Status_Cancelled);
            if (responseDto != null && responseDto.IsSuccess)
            {
                TempData["success"] = "Status updated successfully";
                return RedirectToAction(nameof(OrderDetail), new {  orderId });
            }
            TempData["error"] = "Error encountered!";
            return RedirectToAction(nameof(OrderDetail), new {  orderId });
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
