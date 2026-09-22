using Mango.Web.Models;
using Mango.Web.Service;
using Mango.Web.Service.IService;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;

namespace Mango.Web.Controllers
{
    public class AuthController : Controller
    {
        private readonly IAuthSerivce _authSerivce;
        private readonly ITokenService _tokenService;
        public AuthController(IAuthSerivce authSerivce, ITokenService tokenService)
        {
            _authSerivce = authSerivce;
            _tokenService = tokenService;
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
                await SignInUser(loginResponseDto);
                _tokenService.SetToken(loginResponseDto.Token);
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


        [HttpPost]
        public async Task<IActionResult> Register(RegisterationRequestDto obj)
        {
            ResponseDto? responseDto = await _authSerivce.RegisterAsync(obj);
            if (responseDto != null && responseDto.IsSuccess)
            {
                // Handle the successful response
                TempData["success"] = "Registration Successful";
                return RedirectToAction(nameof(Login));
            }
            else
            {
                TempData["error"] = responseDto?.ErrorMessage;
                return View(obj);
            }
        }

        public async Task<IActionResult> Logout()
        {
            await HttpContext.SignOutAsync();
            _tokenService.ClearToken();
            return RedirectToAction("Index", "Home");
        }

        private async Task SignInUser(LoginResponseDto model)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var jwt = tokenHandler.ReadJwtToken(model.Token);

            var identity = new ClaimsIdentity(CookieAuthenticationDefaults.AuthenticationScheme);
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Sub,
                jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Sub).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.PhoneNumber,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.PhoneNumber).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.Name,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.Name).Value));
            identity.AddClaim(new Claim(JwtRegisteredClaimNames.GivenName,
               jwt.Claims.FirstOrDefault(u => u.Type == JwtRegisteredClaimNames.GivenName).Value));
            identity.AddClaim(new Claim(ClaimTypes.Role,
               jwt.Claims.FirstOrDefault(u => u.Type == "role").Value));

            var principal = new ClaimsPrincipal(identity);
            await HttpContext.SignInAsync(CookieAuthenticationDefaults.AuthenticationScheme, principal);

        }
    }
}
