using System.ComponentModel.DataAnnotations;

namespace Mango.ShoppingCartAPI.Models
{
    public class CartHeader
    {
        [Key]
        public int CartHeaderId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [MaxLength(100)]
        public string? CouponCode { get; set; }

    }
}
