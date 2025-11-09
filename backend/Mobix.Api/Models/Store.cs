using System.ComponentModel.DataAnnotations;

namespace Mobix.Api.Models
{
    public class Store
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } 

        [Required]
        [StringLength(255)]
        public string BaseUrl { get; set; }

        public ICollection<PriceHistory> PriceHistories { get; set; } = new List<PriceHistory>();
    }
}