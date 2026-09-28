using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.Web.Models
{
    public class OrderDetailsDto
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }

        public int ProductId { get; set; }
        public ProductDto? Product { get; set; }

        [Range(1, 100, ErrorMessage = "Count must be between 1 and 100.")]
        public int Count { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public double Price { get; set; }
    }
}
