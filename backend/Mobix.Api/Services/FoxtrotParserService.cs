using HtmlAgilityPack;
using Mobix.Api.DTOs;
using Mobix.Api.Models;
using System.Web;

namespace Mobix.Api.Services
{
    public class FoxtrotParserService : BaseParserService, IStoreParserService
    {
        public FoxtrotParserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public string StoreName => "Foxtrot";
        private const decimal MIN_SMARTPHONE_PRICE = 2000; 

        public async Task<ParserResult> FindPriceAsync(Smartphone smartphone, string storeBaseUrl)
        {
            try
            {
                string searchTerm = HttpUtility.UrlEncode(smartphone.Name);
                string searchUrl = $"{storeBaseUrl}uk/search/?query={searchTerm}";

                HtmlDocument doc = await FetchHtmlDocument(searchUrl);
                if (doc == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не вдалося завантажити сторінку Foxtrot"
                    };
                }

                var productNodes = doc.DocumentNode.SelectNodes("//div[contains(@class, 'sc-product') and contains(@class, 'card')]");
                if (productNodes == null || productNodes.Count == 0)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Товари не знайдено за запитом"
                    };
                }

                string normalizedSmartphoneName = smartphone.Name.ToLower().Replace(" ", "");

                foreach (var node in productNodes)
                {
                    var nameNode = node.SelectSingleNode(".//a[contains(@class, 'card__title')]");
                    string foundName = nameNode != null ? SanitizeString(nameNode.InnerText) : string.Empty;
                    string normalizedFoundName = foundName.ToLower().Replace(" ", "");

                    if (!normalizedFoundName.Contains(normalizedSmartphoneName) &&
                        !normalizedFoundName.Contains(normalizedSmartphoneName.Split(' ')[0]))
                    {
                        continue;
                    }

                    var priceNode = node.SelectSingleNode(".//div[contains(@class, 'card__head')]");
                    if (priceNode == null) continue;

                    string rawPrice = priceNode.GetAttributeValue("data-price", "0");

                    if (decimal.TryParse(rawPrice, out decimal price) && price >= MIN_SMARTPHONE_PRICE)
                    {
                        string rawUrl = nameNode?.GetAttributeValue("href", null);
                        string productUrl = BuildFullUrl(storeBaseUrl, rawUrl);

                        return new ParserResult
                        {
                            IsSuccess = true,
                            Price = price,
                            FoundProductName = foundName,
                            ProductUrl = productUrl
                        };
                    }
                }

                return new ParserResult
                {
                    IsSuccess = false,
                    ErrorMessage = $"Релевантний смартфон (ціна > {MIN_SMARTPHONE_PRICE} грн) не знайдено на сторінці пошуку Foxtrot."
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
