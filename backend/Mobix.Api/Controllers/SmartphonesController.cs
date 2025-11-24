using Microsoft.AspNetCore.Mvc;
using Mobix.Api.Data;
using Microsoft.EntityFrameworkCore;
using Mobix.Api.DTOs;
using System.Linq;
using System;

namespace Mobix.Api.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class SmartphonesController : ControllerBase
    {
        private readonly ApplicationDbContext _context;

        public SmartphonesController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public async Task<IActionResult> GetSmartphones(
            [FromQuery] string sortBy, 
            [FromQuery] string manufacturer,
            [FromQuery] decimal? minPrice,
            [FromQuery] decimal? maxPrice)
        {
            var smartphonesQuery = _context.Smartphones.AsQueryable();

            if (!string.IsNullOrEmpty(manufacturer))
            {
                var selectedManufacturers = manufacturer.Split(',')
                    .Select(s => s.Trim().ToLower())
                    .ToList();
                
                if (selectedManufacturers.Any())
                {
                    smartphonesQuery = smartphonesQuery.Where(s => selectedManufacturers.Contains(s.Manufacturer.ToLower()));
                }
            }
            
            var cutoffDate = DateTime.UtcNow.AddDays(-1); 
            
            var dataQuery = smartphonesQuery
                .Select(s => new {
                    Smartphone = s,
                    BestPriceEntry = _context.PriceHistories
                                        .Where(ph => ph.SmartphoneId == s.Id && ph.CollectionDate >= cutoffDate)
                                        .OrderBy(ph => ph.Price)
                                        .Include(ph => ph.Store)
                                        .FirstOrDefault(),
                    MaxPriceValue = _context.PriceHistories
                                        .Where(ph => ph.SmartphoneId == s.Id && ph.CollectionDate >= cutoffDate)
                                        .Max(ph => (decimal?)ph.Price) 
                })
                .Where(data => data.BestPriceEntry != null); 

            if (minPrice.HasValue && minPrice.Value > 0)
            {
                dataQuery = dataQuery.Where(data => data.BestPriceEntry.Price >= minPrice.Value); 
            }

            if (maxPrice.HasValue && maxPrice.Value > 0)
            {
                dataQuery = dataQuery.Where(data => data.MaxPriceValue.HasValue && data.MaxPriceValue.Value <= maxPrice.Value);
            }

            switch (sortBy?.ToLower())
            {
                case "name":
                    dataQuery = dataQuery.OrderBy(d => d.Smartphone.Name);
                    break;
                case "cheap":
                    dataQuery = dataQuery.OrderBy(d => d.BestPriceEntry.Price);
                    break;
                case "expensive":
                    dataQuery = dataQuery.OrderByDescending(d => d.BestPriceEntry.Price);
                    break;
                case null:
                case "":
                    dataQuery = dataQuery.OrderBy(d => d.Smartphone.Name);
                    break;
                default:
                    return BadRequest(new { message = $"Невірний параметр сортування: {sortBy}. Доступно: name, cheap, expensive." });
            }

            var smartphoneDtos = await dataQuery
                .Select(data => new SmartphoneDto
                {
                    Id = data.Smartphone.Id,
                    Name = data.Smartphone.Name,
                    Manufacturer = data.Smartphone.Manufacturer,
                    ImageUrl = data.Smartphone.ImageUrl,
                    ImageUrl2 = data.Smartphone.ImageUrl2,
                    ImageUrl3 = data.Smartphone.ImageUrl3,
                    MinPrice = data.BestPriceEntry.Price,
                    StoreName = data.BestPriceEntry.Store.Name,
                    StoreUrl = data.BestPriceEntry.ProductUrl,
                    MaxPrice = data.MaxPriceValue ?? data.BestPriceEntry.Price ,
                    Ram = data.Smartphone.Ram, 
                    DisplaySize = data.Smartphone.DisplaySize,
                    DisplayHz = data.Smartphone.DisplayHz,
                    Storage = data.Smartphone.Storage
                })
                .ToListAsync();

            return Ok(smartphoneDtos);
        }
    }
}