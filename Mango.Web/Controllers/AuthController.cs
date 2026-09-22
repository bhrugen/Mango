using Mango.Web.Models;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthSerivce authSerivce;

        public AuthController(IAuthSerivce authSerivce)
        {
            this.authSerivce = authSerivce;
        }

        [HttpGet]
        public IActionResult Login()
        {
            LoginRequestDto loginRequestDto = new();
            return View(loginRequestDto);
        }

        [HttpGet]
        public IActionResult Register()
        {
            RegisterationRequestDto registerationRequestDto = new();
            return View(registerationRequestDto);
        }
    }
}
