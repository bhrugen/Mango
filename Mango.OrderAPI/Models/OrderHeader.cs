using System.ComponentModel.DataAnnotations;

namespace Mango.OrderAPI.Models
{
    public class OrderHeader
    {
        [Key]
        public int OrderHeaderId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [MaxLength(100)]
        public string? CouponCode { get; set; }

        [Required]
        public string? Name { get; set; }
        [Required]
        public string? Phone { get; set; }
        [Required]
        public string? Email { get; set; }

        public DateTime OrderTime { get; set; }
        public string? Status{ get; set; }

        public double Discount { get; set; }
        [Range(1, 100, ErrorMessage = "Count must be between 1 and 10000.")]
        public double OrderTotal { get; set; }

        public IEnumerable<OrderDetails>? OrderDetails { get; set; }

    }
}
