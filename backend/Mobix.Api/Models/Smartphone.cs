using System.ComponentModel.DataAnnotations;
using System.Collections.Generic;

namespace Mobix.Api.Models
{
    public class Smartphone
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; }

        [StringLength(50)]
        public string Manufacturer { get; set; }
        
        [StringLength(500)]
        public string ImageUrl { get; set; }

        [StringLength(500)]
        public string ImageUrl2 { get; set; } 

        [StringLength(500)]
        public string ImageUrl3 { get; set; } 
        [StringLength(10)]
        public string Ram { get; set; }

        [StringLength(10)]
        public string DisplaySize { get; set; }

        public int? DisplayHz { get; set; }
        
        [StringLength(10)]
        public string Storage { get; set; }

        public ICollection<PriceHistory> PriceHistories { get; set; }
    }
}