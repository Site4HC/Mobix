namespace Mobix.Api.DTOs
{
    public class SmartphoneDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
        public string Manufacturer { get; set; }
        public string ImageUrl { get; set; }

        public decimal MinPrice { get; set; }
        public string StoreName { get; set; } 
        public string StoreUrl { get; set; }  
        public string ImageUrl2 { get; set; }
        public string ImageUrl3 { get; set; }
    }
}