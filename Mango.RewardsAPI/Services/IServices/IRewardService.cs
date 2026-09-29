
using Mango.RewardsAPI.Models.Dto;

namespace Mango.RewardsAPI.Services.IServices
{
    public interface IRewardService
    {
        Task UpdateRewards(OrderHeaderDto orderHeaderDto);
    }
}
