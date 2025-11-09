using HtmlAgilityPack;
using Mobix.Api.DTOs;
using Mobix.Api.Models;
using System.Web;
using System.Text.RegularExpressions;
using System.Linq;

namespace Mobix.Api.Services
{
    public class CitrusParserService : BaseParserService, IStoreParserService
    {
        public CitrusParserService(IHttpClientFactory httpClientFactory) : base(httpClientFactory)
        {
        }

        public string StoreName => "Citrus";
        private const decimal MIN_SMARTPHONE_PRICE = 2000;

        public async Task<ParserResult> FindPriceAsync(Smartphone smartphone, string storeBaseUrl)
        {
            try
            {
                string searchTerm = HttpUtility.UrlEncode(smartphone.Name);
                string searchUrl = $"{storeBaseUrl}search/?query={searchTerm}";

                HtmlDocument doc = await FetchHtmlDocument(searchUrl);
                if (doc == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не вдалося завантажити сторінку Citrus"
                    };
                }

                var productNodes = doc.DocumentNode.SelectNodes(
                    "//div[contains(@class, 'MainProductCard-module__root')]");

                if (productNodes == null || productNodes.Count == 0)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Товар (смартфон) не знайдено"
                    };
                }

                decimal minPrice = decimal.MaxValue;
                string minPriceUrl = null;
                string minPriceName = null;

                foreach (var productNode in productNodes)
                {
                    var titleNode = productNode.SelectSingleNode(
                        ".//span[contains(@class, 'MainProductCard-module__title')]");
                    if (titleNode == null) continue;

                    string foundName = SanitizeString(titleNode.InnerText);

                    var smartphoneKeywords = smartphone.Name.ToLower().Split(' ');
                    bool isMatch = smartphoneKeywords.All(k => foundName.ToLower().Contains(k));
                    if (!isMatch) continue;

                    var priceNode = productNode.SelectSingleNode(
                        ".//span[contains(@class, 'MainProductCard-module__price')]");
                    if (priceNode == null) continue;

                    string rawPrice = priceNode.GetAttributeValue("data-price", "0");
                    if (!decimal.TryParse(rawPrice, out decimal price)) continue;

                    if (price < MIN_SMARTPHONE_PRICE) continue;

                    var linkNode = productNode.SelectSingleNode(
                        ".//a[contains(@class, 'MainProductCard-module__link')]");
                    string rawUrl = linkNode?.GetAttributeValue("href", null);
                    string productUrl = BuildFullUrl(storeBaseUrl, rawUrl);

                    if (price < minPrice)
                    {
                        minPrice = price;
                        minPriceUrl = productUrl;
                        minPriceName = foundName;
                    }
                }

                if (minPriceUrl == null)
                {
                    return new ParserResult
                    {
                        IsSuccess = false,
                        ErrorMessage = "Не знайдено жодного товару з допустимою ціною"
                    };
                }

                return new ParserResult
                {
                    IsSuccess = true,
                    Price = minPrice,
                    FoundProductName = minPriceName,
                    ProductUrl = minPriceUrl
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
