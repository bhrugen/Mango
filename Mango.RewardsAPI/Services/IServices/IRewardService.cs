
using Mango.Web.Models;

namespace Mango.RewardsAPI.Services.IServices
{
    public interface IRewardService
    {
        Task UpdateRewards(OrderHeaderDto orderHeaderDto);
    }
}
