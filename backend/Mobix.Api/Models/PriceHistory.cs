using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Mobix.Api.Models
{
    public class PriceHistory
    {
        public int Id { get; set; }

        [Column(TypeName = "decimal(18,2)")]
        public decimal Price { get; set; }

        public DateTime CollectionDate { get; set; }
        
        [StringLength(500)]
        public string ProductUrl { get; set; } 
        
        public int SmartphoneId { get; set; }
        public int StoreId { get; set; }

        public Smartphone Smartphone { get; set; }
        public Store Store { get; set; }
    }
}