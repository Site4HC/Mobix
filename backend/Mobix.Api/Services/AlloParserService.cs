using HtmlAgilityPack;
using Mobix.Api.DTOs;
using Mobix.Api.Models;
using System.Web;

namespace Mobix.Api.Services
{
    public class AlloParserService : BaseParserService, IStoreParserService
    {
        public AlloParserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public string StoreName => "Allo";
        private const decimal MIN_SMARTPHONE_PRICE = 2000; 

        public async Task<ParserResult> FindPriceAsync(Smartphone smartphone, string storeBaseUrl)
        {
            try
            {
                string searchTerm = HttpUtility.UrlEncode(smartphone.Name);
                string searchUrl = $"{storeBaseUrl}ua/catalogsearch/result/?q={searchTerm}";

                HtmlDocument doc = await FetchHtmlDocument(searchUrl);
                if (doc == null)
                {
                    return new ParserResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = "Не вдалося завантажити сторінку Allo" 
                    };
                }

                var productNode = doc.DocumentNode.SelectSingleNode(
                    "//div[contains(@class, 'product-card') and .//a[contains(@class, 'product-card__title')] and .//span[contains(@class, 'sum')]]");

                if (productNode == null)
                {
                    return new ParserResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = "Товар (смартфон) не знайдено" 
                    };
                }

                var priceNode = productNode.SelectSingleNode(".//div[contains(@class,'v-pb__cur')]/span[contains(@class,'sum')]");
                if (priceNode == null)
                {
                    priceNode = productNode.SelectSingleNode(".//span[contains(@class,'sum')]");
                }
                
                if (priceNode == null)
                {
                    return new ParserResult 
                    { 
                        IsSuccess = false, 
                        ErrorMessage = "Ціну не знайдено" 
                    };
                }

                var nameNode = productNode.SelectSingleNode(".//a[contains(@class, 'product-card__title')]");

                string rawPrice = SanitizePrice(priceNode.InnerText);
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
