using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Mobix.Api.Data;
using Mobix.Api.Models;
using Mobix.Api.Services;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

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

            while (!stoppingToken.IsCancellationRequested)
            {
                _logger.LogInformation("Починаємо цикл оновлення цін...");
                await RunParsingCycle(stoppingToken);

                await Task.Delay(TimeSpan.FromHours(24), stoppingToken);
            }

            _logger.LogInformation("Фонова служба оновлення цін зупинена.");
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

                    var newPriceEntries = new List<PriceHistory>();

                    foreach (var smartphone in smartphones)
                    {
                        foreach (var store in stores)
                        {
                            if (stoppingToken.IsCancellationRequested) return;

                            if (!parserLookup.TryGetValue(store.Name, out var parser))
                            {
                                continue;
                            }

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
                                newPriceEntries.Add(priceHistoryEntry);
                                
                                _logger.LogInformation($"УСПІХ: {smartphone.Name} @ {store.Name} - {result.Price.ToString("N0")} грн.");
                            }
                            else
                            {
                                _logger.LogWarning($"ПОМИЛКА: {smartphone.Name} @ {store.Name} - {result.ErrorMessage}");
                            }
                        }
                    }

                    if (newPriceEntries.Any())
                    {
                        dbContext.PriceHistories.AddRange(newPriceEntries);
                        await dbContext.SaveChangesAsync(stoppingToken);
                        _logger.LogInformation($"Всі нові ціни успішно збережено в БД.");
                    }
                }
                catch (DbUpdateException ex)
                {
                    _logger.LogCritical(ex, "Критична помилка під час збереження даних. Перевірте структуру БД.");
                }
                catch (Exception ex)
                {
                    _logger.LogCritical(ex, "Критична помилка під час циклу парсингу.");
                }
                _logger.LogInformation("Цикл оновлення цін завершено.");
            }
        }
    }
}