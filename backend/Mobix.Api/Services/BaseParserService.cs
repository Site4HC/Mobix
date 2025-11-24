using HtmlAgilityPack;
using System.Text.RegularExpressions;
using System.Net.Http; 
using System.Threading.Tasks;
using System;
using System.Net; 

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
                
                httpClient.DefaultRequestHeaders.Clear();
                httpClient.DefaultRequestHeaders.Add("User-Agent", "Mozilla/5.0 (Windows NT 10.0; Win64; x64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/120.0.0.0 Safari/537.36");
                httpClient.DefaultRequestHeaders.Add("Accept", "text/html,application/xhtml+xml,application/xml;q=0.9,image/avif,image/webp,*/*;q=0.8");
                httpClient.DefaultRequestHeaders.Add("Accept-Language", "en-US,en;q=0.5");
                httpClient.DefaultRequestHeaders.Add("Accept-Encoding", "gzip, deflate, br");
                httpClient.DefaultRequestHeaders.Add("Connection", "keep-alive");
                httpClient.DefaultRequestHeaders.Add("Sec-Fetch-Dest", "document");
                
                var response = await httpClient.GetAsync(url);

                if (response.StatusCode == HttpStatusCode.Forbidden)
                {
                    Console.WriteLine($"Помилка: Запит до {url} заблоковано (403 Forbidden).");
                    return null;
                }

                response.EnsureSuccessStatusCode(); 
                
                var html = await response.Content.ReadAsStringAsync();
                
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

            var sanitized = WebUtility.HtmlDecode(input); 
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
            if (string.IsNullOrWhiteSpace(relativeUrl))
                return baseUrl;

            if (relativeUrl.StartsWith("http", StringComparison.OrdinalIgnoreCase))
                return relativeUrl;

            try
            {
                var baseUri = new Uri(baseUrl.TrimEnd('/'));
                var fullUri = new Uri(baseUri, relativeUrl.TrimStart('/'));
                return fullUri.ToString();
            }
            catch
            {
                return $"{baseUrl.TrimEnd('/')}/{relativeUrl.TrimStart('/')}";
            }
        }
    }
}