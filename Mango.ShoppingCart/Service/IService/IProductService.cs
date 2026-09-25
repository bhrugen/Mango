using Mango.ShoppingCartAPI.Models.Dto;

namespace Mango.ShoppingCartAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
