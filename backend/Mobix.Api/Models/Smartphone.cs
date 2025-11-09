using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.Models
{
    public class Smartphone
    {
        public int Id { get; set; }

        [Required]
        [StringLength(200)]
        public string Name { get; set; } 

        [StringLength(100)]
        public string Manufacturer { get; set; } 

        [StringLength(500)]
        public string ImageUrl { get; set; }

        public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
    }
}