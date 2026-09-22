using Mango.Web.Models;

namespace Mango.Web.Service.IService
{
    public interface IAuthSerivce
    {
        Task<ResponseDto?> LoginAsync(LoginRequestDto loginRequestDto);
        Task<ResponseDto?> RegisterAsync(RegisterationRequestDto registerationRequestDto);
    }
}
