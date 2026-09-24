using System.ComponentModel.DataAnnotations;

namespace Mango.Web.Models
{
    public class ProductDto
    {
        public int ProductId { get; set; }
        [Required, StringLength(100)]
        public string Name { get; set; }
        [Required, Range(1, 1000)]
        public double Price { get; set; }
        public string? Description { get; set; }
        public string? CategoryName { get; set; }
        public string? ImageUrl { get; set; }
        public IFormFile? Image { get; set; }
    }
}
