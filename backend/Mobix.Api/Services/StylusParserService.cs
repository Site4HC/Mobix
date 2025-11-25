using HtmlAgilityPack;
using Mobix.Api.DTOs;
using Mobix.Api.Models;
using System.Web;


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
                string searchTerm = HttpUtility.UrlEncode(smartphone.Name);
                string searchUrl = $"{storeBaseUrl}search?query={searchTerm}";

                var doc = await FetchHtmlDocument(searchUrl);
                if (doc == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не вдалося завантажити сторінку Stylus"
                    };
                }

                var productNode = doc.DocumentNode.SelectSingleNode(
                    "//div[contains(@class, 'product-item')]" +
                    "[.//a[contains(@class, 'product-item__title')]]" +
                    "[.//div[contains(@class, 'product-item__price')]]"
                );

                if (productNode == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Товар (смартфон) не знайдено на Stylus"
                    };
                }

                var priceNode = productNode.SelectSingleNode(".//div[contains(@class,'price') or contains(@class,'product-item__price')]");

                if (priceNode == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Ціну не знайдено"
                    };
                }

                string rawPrice = SanitizePrice(priceNode.InnerText);

                var nameNode = productNode.SelectSingleNode(".//a[contains(@class,'product-item__title')]");
                string foundName = (nameNode != null)
                    ? SanitizeString(nameNode.InnerText)
                    : smartphone.Name;

                string rawUrl = nameNode?.GetAttributeValue("href", null);
                string productUrl = BuildFullUrl(storeBaseUrl, rawUrl);

                if (!decimal.TryParse(rawPrice, out decimal price) || price < MIN_SMARTPHONE_PRICE)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = $"Невдале розпізнавання ціни < {MIN_SMARTPHONE_PRICE}. Знайдено: {rawPrice}"
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
