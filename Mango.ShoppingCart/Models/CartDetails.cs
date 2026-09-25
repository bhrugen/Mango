using Mango.ShoppingCartAPI.Models.Dto;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mango.ShoppingCartAPI.Models
{
    public class CartDetails
    {
        [Key]
        public int CartDetailsId { get; set; }
        public int CartHeaderId { get; set; }
        [ForeignKey("CartHeaderId")]
        public CartHeader CartHeader { get; set; }

        public int ProductId { get; set; }
        [NotMapped]
        public ProductDto? Product { get; set; }

        [Range(1, 100, ErrorMessage = "Count must be between 1 and 100.")]
        public int Count { get; set; }
    }
}
