using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class CouponDto
    {
        public int CouponId { get; set; }
        [Required, StringLength(100)]
        public string CouponCode { get; set; }
        [Required, Range(0.01, 10000)]
        public double DiscountAmount { get; set; }
        public int MinAmount { get; set; }
    }
}
