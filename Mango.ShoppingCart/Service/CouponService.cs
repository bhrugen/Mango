using Mango.Serives.Shared.Models;
using Mango.ShoppingCartAPI.Models.Dto;
using Mango.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.ShoppingCartAPI.Service
{
    public class CouponService : ICouponService
    {

        private readonly IHttpClientFactory _httpClient;
        public CouponService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<CouponDto> GetCoupon(string couponCode)
        {
            var client = _httpClient.CreateClient("Coupon");
            var response = await client.GetAsync($"/api/coupons/GetByCode/{couponCode}");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(responseDto != null && responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<CouponDto>(Convert.ToString(responseDto.Result));
            }
            return new CouponDto();
        }
    }
}
