using Mango.Serives.Shared.Models;
using Mango.Services.AuthAPI.Models;
using Mango.Services.AuthAPI.Service.IService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;

namespace Mango.Services.AuthAPI.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthAPIController : ControllerBase
    {
        private readonly UserManager<ApplicationUser> _userManager;
        private readonly RoleManager<IdentityRole> _roleManager;
        private readonly IJwtTokenGenerator _jwtTokenGenerator;
        private const string CustomerRole = "CUSTOMER";
        private const string AdminRole = "ADMIN";
        private readonly ResponseDto _responseDto;


        public AuthAPIController(UserManager<ApplicationUser> userManager,
            IJwtTokenGenerator jwtTokenGenerator,
            RoleManager<IdentityRole> roleManager)
        {
            _userManager = userManager;
            _responseDto = new ResponseDto();
            _roleManager = roleManager;
            _jwtTokenGenerator = jwtTokenGenerator;
        }

        [HttpPost("register")]
        public async Task<IActionResult> Register([FromBody] RegisterationRequestDto model)
        {
            var roleToAssign = string.IsNullOrEmpty(model.Role) ? CustomerRole : model.Role.ToUpper();
            if (roleToAssign != CustomerRole)
            {
                //add one more check, later on.
                if (roleToAssign != AdminRole)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.ErrorMessage = "Invalid role specified.";
                    return BadRequest(_responseDto);
                }
            }

            ApplicationUser user = new()
            {
                UserName = model.Email,
                Email = model.Email,
                Name = model.Name,
                PhoneNumber = model.PhoneNumber
            };


            try
            {
                var result = await _userManager.CreateAsync(user, model.Password);
                if (!result.Succeeded)
                {
                    _responseDto.IsSuccess = false;
                    _responseDto.ErrorMessage = "User creation failed. Please check the details and try again.";
                    return BadRequest(_responseDto);
                }
            }
            catch (Exception ex)
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage = "Error Encountered";
                return StatusCode(StatusCodes.Status500InternalServerError, _responseDto);
            }


            if(!await _roleManager.RoleExistsAsync(roleToAssign))
            {
                await _roleManager.CreateAsync(new IdentityRole(roleToAssign));
            }

            await _userManager.AddToRoleAsync(user, roleToAssign);

            return Ok(_responseDto);
        }

        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginRequestDto model)
        {

            var user = await _userManager.FindByNameAsync(model.Email);

            if(user==null || !await _userManager.CheckPasswordAsync(user, model.Password))
            {
                _responseDto.IsSuccess = false;
                _responseDto.ErrorMessage= "Email or password is incorrect";
                return BadRequest(_responseDto);
            }

            _responseDto.Result = new LoginResponseDto()
            {
                Token = _jwtTokenGenerator.GenerateToken(user, await _userManager.GetRolesAsync(user))
            };

            return Ok(_responseDto);
        }
    }
}
