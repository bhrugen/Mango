using Mango.OrderAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.OrderAPI.Models
{
    public class OrderDetails
    {
        [Key]
        public int OrderDetailsId { get; set; }
        public int OrderHeaderId { get; set; }
        [ForeignKey("OrderHeaderId")]
        public OrderHeaderDto? OrderHeader { get; set; }

        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }

        [Range(1, 100, ErrorMessage = "Count must be between 1 and 100.")]
        public int Count { get; set; }
        [Required]
        public string? ProductName { get; set; }
        public double Price { get; set; }
    }
}
