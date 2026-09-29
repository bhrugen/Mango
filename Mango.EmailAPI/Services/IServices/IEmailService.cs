using Mango.EmailAPI.Models.Dto;

namespace Mango.EmailAPI.Services.IServices
{
    public interface IEmailService
    {
        Task EmailCartAndLog(CartDto cartDto);
        Task OrderCreatedEmailAndLog(OrderHeaderDto orderHeaderDto);
        Task RegisterUserEmailAndLog(string email);
    }
}
