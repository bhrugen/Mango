using Mango.Serives.Shared.Models;
using Mango.Services.AuthAPI.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    public class AuthAPIController : Controller
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private const string CustomerRole = "CUSTOMER";
        private const string AdminRole = "ADMIN";
        private readonly ResponseDto _responseDto;


        public AuthAPIController(UserManager<ApplicationUser> userManager, RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _responseDto = new ResponseDto();
            _roleManager = roleManager;
        }

        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            return View();
        }

        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {
            return View();
        }
    }
}
