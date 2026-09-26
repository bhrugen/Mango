using System.ComponentModel.DataAnnotations;
namespace Mango.Web.Models
{
    public class CartHeaderDto
    {
        [Required]
        public int CartHeaderId { get; set; }
        [Required]
        public string? UserId { get; set; }
        [MaxLength(100)]
        public string? CouponCode { get; set; }
        public double Discount { get; set; }
        public double CartTotal { get; set; }

    }
}
