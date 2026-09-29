using System.ComponentModel.DataAnnotations;

namespace Mango.RewardsAPI.Models
{
    public class Rewards
    {
        public int Id { get; set; }
        [Required]
        public string? UserId { get; set; }
        [Required]
        public DateTime RewardsDate { get; set; }
        [Required]
        public int RewardsActivity { get; set; }
        [Required]
        public int OrderId { get; set; }
    }
}
