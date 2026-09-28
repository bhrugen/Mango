using Mango.OrderAPI.Models.Dto;


namespace Mango.OrderAPI.Service.IService
{
    public interface IProductService
    {
        Task<IEnumerable<ProductDto>> GetProducts();
    }
}
