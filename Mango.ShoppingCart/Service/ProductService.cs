using Mango.Serives.Shared.Models;
using Mango.ShoppingCartAPI.Models.Dto;
using Mango.ShoppingCartAPI.Service.IService;
using Newtonsoft.Json;

namespace Mango.ShoppingCartAPI.Service
{
    public class ProductService : IProductService
    {

        private readonly IHttpClientFactory _httpClient;
        public ProductService(IHttpClientFactory httpClient)
        {
            _httpClient = httpClient;
        }

        public async Task<IEnumerable<ProductDto>> GetProducts()
        {
            var client = _httpClient.CreateClient("Product");
            var response = await client.GetAsync($"/api/product");
            var apiContent = await response.Content.ReadAsStringAsync();
            var responseDto = JsonConvert.DeserializeObject<ResponseDto>(apiContent);
            if(responseDto != null && responseDto.IsSuccess)
            {
                return JsonConvert.DeserializeObject<IEnumerable<ProductDto>>(Convert.ToString(responseDto.Result));
            }
            return new List<ProductDto>();
        }
    }
}
