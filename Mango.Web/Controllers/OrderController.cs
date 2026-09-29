using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;

namespace Mango.Web.Controllers
{
    [Authorize]
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
            OrderHeaderDto? orderHeaderDto = await LoadOrder(orderId);
            if (orderHeaderDto == null)
            {
                TempData["error"] = "Order not found or you do not have access to it.";
                return RedirectToAction(nameof(OrderIndex));
            }
            return View(orderHeaderDto);
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> OrderReadyForPickup(int orderId)
        {
            return await ChangeStatus(orderId, SD.Status_ReadyForPickup, "Order is ready for pickup");
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> CompleteOrder(int orderId)
        {
            return await ChangeStatus(orderId, SD.Status_Completed, "Order completed");
        }

        [HttpPost]
        [Authorize(Roles = SD.RoleAdmin)]
        public async Task<IActionResult> CancelOrder(int orderId)
        {
            return await ChangeStatus(orderId, SD.Status_Cancelled, "Order cancelled");
        }

        [HttpGet]
        public async Task<IActionResult> GetAll(string? status)
        {
            // Admins see every order; everyone else only their own.
            string? userId = User.IsInRole(SD.RoleAdmin)
                ? null
                : User.Claims.FirstOrDefault(u => u.Type == "sub")?.Value;

            IEnumerable<OrderHeaderDto> list = new List<OrderHeaderDto>();
            if (User.IsInRole(SD.RoleAdmin) || !string.IsNullOrEmpty(userId))
            {
                ResponseDto? response = await _orderService.GetAllOrder(userId);
                if (response != null && response.IsSuccess)
                {
                    list = JsonConvert.DeserializeObject<List<OrderHeaderDto>>(Convert.ToString(response.Result))
                           ?? new List<OrderHeaderDto>();
                }
            }

            list = status?.ToLower() switch
            {
                "approved" => list.Where(u => u.Status == SD.Status_Approved),
                "readyforpickup" => list.Where(u => u.Status == SD.Status_ReadyForPickup),
                "cancelled" => list.Where(u => u.Status == SD.Status_Cancelled || u.Status == SD.Status_Refunded),
                _ => list
            };

            return Json(new { data = list });
        }

        private async Task<IActionResult> ChangeStatus(int orderId, string newStatus, string successMessage)
        {
            ResponseDto? response = await _orderService.UpdateOrderStatus(orderId, newStatus);
            if (response != null && response.IsSuccess)
            {
                TempData["success"] = successMessage;
            }
            else
            {
                TempData["error"] = response?.ErrorMessage ?? "Status update failed";
            }
            return RedirectToAction(nameof(OrderDetail), new { orderId });
        }

        private async Task<OrderHeaderDto?> LoadOrder(int orderId)
        {
            ResponseDto? response = await _orderService.GetOrderById(orderId);
            if (response != null && response.IsSuccess)
            {
                return JsonConvert.DeserializeObject<OrderHeaderDto>(Convert.ToString(response.Result));
            }
            return null;
        }
    }
}
