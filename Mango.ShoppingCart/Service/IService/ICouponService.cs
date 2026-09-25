using Mango.ShoppingCartAPI.Models.Dto;

namespace Mango.ShoppingCartAPI.Service.IService
{
    public interface ICouponService
    {
        Task<CouponDto> GetCoupon(string couponCode);
    }
}
