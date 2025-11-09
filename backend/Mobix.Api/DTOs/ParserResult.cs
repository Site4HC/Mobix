namespace Mobix.Api.DTOs
{
    public class ParserResult
    {
        public bool IsSuccess { get; set; }
        public decimal Price { get; set; }
        public string FoundProductName { get; set; } 
        public string ProductUrl { get; set; } 
        public string ErrorMessage { get; set; } 
    }
}