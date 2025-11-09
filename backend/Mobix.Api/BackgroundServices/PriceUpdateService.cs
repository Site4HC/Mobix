using Microsoft.EntityFrameworkCore;
using Mobix.Api.Data;
using Mobix.Api.Models;
using Mobix.Api.Services;

namespace Mobix.Api.BackgroundServices
{
    public class PriceUpdateService : BackgroundService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly ILogger<PriceUpdateService> _logger;

        public PriceUpdateService(IServiceProvider serviceProvider, ILogger<PriceUpdateService> logger)
        {
            _serviceProvider = serviceProvider;
            _logger = logger;
        }

        protected override async Task ExecuteAsync(CancellationToken stoppingToken)
        {
            _logger.LogInformation("Фонова служба оновлення цін запущена.");

            await Task.Delay(TimeSpan.FromSeconds(5), stoppingToken);

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Починаємо цикл оновлення цін...");

                await RunParsingCycle(stoppingToken);

                _logger.LogInformation("Цикл оновлення цін завершено.");

                try
                {
                    await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
                }
                catch (OperationCanceledException)
                {
                    break;
                }
            }
        }

        private async Task RunParsingCycle(CancellationToken stoppingToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var dbContext = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                var parserServices = scope.ServiceProvider.GetRequiredService<IEnumerable<IStoreParserService>>();
                
                var parserLookup = parserServices.ToDictionary(p => p.StoreName, p => p);

                try
                {
                    var stores = await dbContext.Stores.ToListAsync(stoppingToken);
                    var smartphones = await dbContext.Smartphones.ToListAsync(stoppingToken);

                    _logger.LogInformation($"Знайдено {smartphones.Count} смартфонів та {stores.Count} магазинів.");

                    foreach (var smartphone in smartphones)
                    {
                        foreach (var store in stores)
                        {
                            if (stoppingToken.IsCancellationRequested) return;
                           
                            if (!parserLookup.TryGetValue(store.Name, out var parser))
                            {
                                continue;
                            }

                            _logger.LogInformation($"Парсинг: {smartphone.Name} @ {store.Name}");
                            var result = await parser.FindPriceAsync(smartphone, store.BaseUrl);

                            if (result.IsSuccess)
                            {
                                var priceHistoryEntry = new PriceHistory
                                {
                                    SmartphoneId = smartphone.Id,
                                    StoreId = store.Id,
                                    Price = result.Price,
                                    CollectionDate = DateTime.UtcNow,
                                    ProductUrl = result.ProductUrl
                                };
                                dbContext.PriceHistories.Add(priceHistoryEntry);
                                _logger.LogInformation($"УСПІХ: {smartphone.Name} @ {store.Name} - {result.Price} грн.");
                            }
                            else
                            {
                                _logger.LogWarning($"ПОМИЛКА: {smartphone.Name} @ {store.Name} - {result.ErrorMessage}");
                            }
                        }
                    }

                    await dbContext.SaveChangesAsync(stoppingToken);
                    _logger.LogInformation("Всі нові ціни успішно збережено в БД.");
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Критична помилка під час циклу парсингу.");
                }
            }
        }
    }
}