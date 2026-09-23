using Mango.Web.Models;
using Mango.Web.Service.IService;
using Mango.Web.Utility;
using System.Text;
using System.Text.Json;

namespace Mango.Web.Service
{
    public class BaseService : IBaseService
    {
        private readonly IHttpClientFactory _httpClientFactory;
        private ITokenService _tokenService;

        public BaseService(IHttpClientFactory httpClientFactory, ITokenService tokenService)
        {
            _httpClientFactory = httpClientFactory;
            _tokenService = tokenService;
        }

        public async Task<ResponseDto> SendAsync(RequestDto requestDto, bool withBearerToken = true)
        {
            try
            {
                HttpClient client = _httpClientFactory.CreateClient("MangoAPI");
                HttpRequestMessage message = new();

                message.Headers.Add("Accept", "application/json");
                message.RequestUri = new Uri(requestDto.Url!);

                if (withBearerToken)
                {
                    var token = _tokenService.GetToken();
                    if(!string.IsNullOrEmpty(token))
                    {
                        message.Headers.Add("Authorization", $"Bearer {token}");
                    }
                }

                if (requestDto.Data != null)
                {
                    message.Content = new StringContent(JsonSerializer.Serialize(requestDto.Data), Encoding.UTF8, "application/json");
                }
                message.Method = requestDto.ApiType switch
                {
                    SD.ApiType.POST => HttpMethod.Post,
                    SD.ApiType.PUT => HttpMethod.Put,
                    SD.ApiType.DELETE => HttpMethod.Delete,
                    _ => HttpMethod.Get
                };

                HttpResponseMessage? apiResponse = null;
                apiResponse = await client.SendAsync(message);


                var apiContent = await apiResponse.Content.ReadAsStringAsync();
                switch (apiResponse.StatusCode)
                {
                    case System.Net.HttpStatusCode.Unauthorized:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            ErrorMessage = "You are not authorized to access this resource."
                        };
                    case System.Net.HttpStatusCode.Forbidden:
                        return new ResponseDto
                        {
                            IsSuccess = false,
                            ErrorMessage = "You do not have permission to access this resource."
                        };
                }

                var apiResponseDto = JsonSerializer.Deserialize<ResponseDto>(apiContent, new JsonSerializerOptions
                {
                    PropertyNameCaseInsensitive = true
                });
                return apiResponseDto!;
            }
            catch (Exception ex)
            {
                var dto = new ResponseDto
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
                return dto;
            }
        }
    }
}
