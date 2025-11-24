using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net.Http; 
using System.Threading.Tasks;

namespace Mobix.Api.Services
{
    public abstract class BaseParserService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public BaseParserService(IHttpClientFactory httpClientFactory)
        {
            _httpClientFactory = httpClientFactory;
        }

        protected async Task<HtmlDocument> FetchHtmlDocument(string url)
        {
            try
            {
                var httpClient = _httpClientFactory.CreateClient("ParserClient");
                var html = await httpClient.GetStringAsync(url);
                var doc = new HtmlDocument();
                doc.LoadHtml(html);
                return doc;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Помилка при завантаженні {url}: {ex.Message}");
                return null;
            }
        }

        protected string SanitizeString(string input)
        {
            if (string.IsNullOrWhiteSpace(input))
            {
                return string.Empty;
            }

            var sanitized = HtmlEntity.DeEntitize(input);
            sanitized = Regex.Replace(sanitized, @"\s+", " ").Trim();
            return sanitized;
        }

        protected string SanitizePrice(string priceString)
        {
            if (string.IsNullOrWhiteSpace(priceString))
            {
                return "0";
            }
            
            string digitsOnly = Regex.Replace(priceString, @"[^\d]", ""); 
            return string.IsNullOrEmpty(digitsOnly) ? "0" : digitsOnly;
        }

        protected string BuildFullUrl(string baseUrl, string relativeUrl)
        {
            if (string.IsNullOrEmpty(relativeUrl))
                return baseUrl;

            if (relativeUrl.StartsWith("http"))
                return relativeUrl;

            try
            {
                var baseUri = new Uri(baseUrl);
                var fullUri = new Uri(baseUri, relativeUrl);
                return fullUri.ToString();
            }
            catch
            {
                return $"{baseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
            }
        }
    }
}
