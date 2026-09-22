using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthSerivce _authSerivce;

        public AuthController(IAuthSerivce authSerivce)
        {
            _authSerivce = authSerivce;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpPost]
        public async Task<IActionResult> Login(LoginRequestDto obj)
        {
            ResponseDto? responseDto = await _authSerivce.LoginAsync(obj);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                LoginResponseDto loginResponseDto = responseDto.GetResult<LoginResponseDto>();
                return RedirectToAction("Index", "Home");
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
                return View(obj);
            }
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDto registerationRequestDto = new();
            return View(registerationRequestDto);
        }
    }
}
