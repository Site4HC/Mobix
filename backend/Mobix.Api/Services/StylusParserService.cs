using HtmlAgilityPack;
using Mobix.Api.Models;
using System;
using System.Linq;
using System.Web; 
using System.Threading.Tasks;
using Mobix.Api.DTOs; 

namespace Mobix.Api.Services
{
    public class StylusParserService : BaseParserService, IStoreParserService
    {
        public StylusParserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory) { }

        public string StoreName => "Stylus";
        private const decimal MIN_SMARTPHONE_PRICE = 2000;

        public async Task<ParserResult> FindPriceAsync(Smartphone smartphone, string storeBaseUrl)
        {
            try
            {
                string searchTerm = Uri.EscapeDataString(SanitizeString(smartphone.Name)); 
                string searchUrl = $"{storeBaseUrl}search?q={searchTerm}"; 
                
                var doc = await FetchHtmlDocument(searchUrl);
                if (doc == null)
                {
                    return new ParserResult 
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не вдалося завантажити сторінку Stylus (404 або помилка з'єднання)"
                    };
                }

                var productContainer = doc.DocumentNode.SelectSingleNode(
                    "//a[contains(@class, 'product-list-item')][1]"
                );
                
                if (productContainer == null)
                {
                    return new ParserResult 
                    {
                        IsSuccess = false,
                        ErrorMessage = "Товар (смартфон) не знайдено на Stylus"
                    };
                }

                var priceNode = productContainer.SelectSingleNode(
                    ".//div[contains(@class, 'jxAkLj')]" 
                );

                var nameNode = productContainer.SelectSingleNode(
                    ".//div[contains(@class, 'hONXRU')]"
                );

                string rawUrl = productContainer.GetAttributeValue("href", null);

                if (priceNode == null || nameNode == null || string.IsNullOrEmpty(rawUrl))
                {
                    return new ParserResult 
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не вдалося вилучити ціну/назву/URL зі знайденої картки товару."
                    };
                }
                
                string rawPrice = priceNode.InnerText;
                string priceString = SanitizePrice(rawPrice);
                string foundName = SanitizeString(nameNode.InnerText);
                string productUrl = BuildFullUrl(storeBaseUrl, rawUrl);

                if (!decimal.TryParse(priceString, out decimal price) || price < MIN_SMARTPHONE_PRICE)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Невдале розпізнавання ціни (< {MIN_SMARTPHONE_PRICE}). Знайдено: {rawPrice}"
                    };
                }

                return new ParserResult
                {
                    IsSuccess = true,
                    Price = price,
                    FoundProductName = foundName,
                    ProductUrl = productUrl
                };
            }
            catch (Exception ex)
            {
                return new ParserResult
                {
                    IsSuccess = false,
                    ErrorMessage = ex.Message
                };
            }
        }
    }
}