using Mobix.Api.DTOs;
using Mobix.Api.Models;

namespace Mobix.Api.Services
{
    public interface IStoreParserService
    {
        string StoreName { get; }

        Task<ParserResult> FindPriceAsync(Smartphone smartphone, string storeBaseUrl);
    }
}