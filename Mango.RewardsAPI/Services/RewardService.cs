using Mango.RewardsAPI.Data;
using Mango.RewardsAPI.Models;
using Mango.RewardsAPI.Models.Dto;
using Mango.RewardsAPI.Services.IServices;
using Microsoft.EntityFrameworkCore;
using System.Text;

namespace Mango.RewardsAPI.Services
{
    public class RewardService : IRewardService
    {
        private DbContextOptions<ApplicationDbContext> _dbContextOptions;
        public RewardService(DbContextOptions<ApplicationDbContext> dbContextOptions)
        {
            _dbContextOptions = dbContextOptions;
        }

        public async Task UpdateRewards(RewardsDto rewardsDto)
        {
           Rewards rewards = new()
           {
               UserId = rewardsDto.UserId,
               RewardsActivity = rewardsDto.RewardsActivity,
               OrderId = rewardsDto.OrderId,
               RewardsDate = DateTime.Now
           };
            await using var _db = new ApplicationDbContext(_dbContextOptions);
            await _db.Rewards.AddAsync(rewards);
            await _db.SaveChangesAsync();
        }
       
    }
}
