using Mobix.Api.Models;
using Microsoft.EntityFrameworkCore;

namespace Mobix.Api.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                await context.Database.EnsureCreatedAsync();

                if (!await context.Stores.AnyAsync())
                {
                    var stores = new List<Store>
                    {
                        new Store { Name = "Stylus", BaseUrl = "https://stylus.ua/" },
                        new Store { Name = "Foxtrot", BaseUrl = "https://www.foxtrot.com.ua/" },
                        new Store { Name = "Citrus", BaseUrl = "https://www.citrus.com.ua/" },
                        new Store { Name = "Moyo", BaseUrl = "https://www.moyo.ua/" },
                    };
                    await context.Stores.AddRangeAsync(stores);
                }

                if (!await context.Smartphones.AnyAsync())
                {
                    var smartphones = new List<Smartphone>
                    {
                        new Smartphone 
                        { 
                            Name = "Apple iPhone 17 Pro Max 256GB", 
                            Manufacturer = "Apple", 
                            ImageUrl = "https://i.ibb.co/Pz3KBYpN/iphone-17-pro-blue-01-1600x1600.webp", 
                            ImageUrl2 = "https://i.ibb.co/cKBn12D6/iphone-17-pro-blue-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/vCZ6cqX5/iphone-17-pro-blue-03-1600x1600.webp",
                            Ram = "12GB", 
                            Storage = "256GB", 
                            DisplaySize = "6.9", 
                            DisplayHz = 120,
                        },
                        new Smartphone 
                        {   Name = "Apple iPhone 17 Pro 256GB", 
                            Manufacturer = "Apple", 
                            ImageUrl = "https://i.ibb.co/20nKgbcz/iphone-17-pro-orn-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/S4qsrpCT/iphone-17-pro-orn-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/v4SN02m1/iphone-17-pro-orn-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120,
                        },
                        new Smartphone 
                        {   Name = "Apple iPhone 17 256GB", 
                            Manufacturer = "Apple", 
                            ImageUrl = "https://i.ibb.co/xKt1wgmW/iphone-17-blk-011-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/Y74kh6yD/iphone-17-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/qM9mhG3W/iphone-17-blk-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120,
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 Pro Max 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/TqTFjJKd/iphone-16-pro-dsrt-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/gZLBSFNt/iphone-16-pro-dsrt-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/bRg4PXyG/iphone-16-pro-dsrt-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.9",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 Pro 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/27xQrckG/iphone-16-pro-wht-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/mCwk2PYR/iphone-16-pro-wht-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Z6m1RnFq/iphone-16-pro-wht-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/GQZwS3Rz/iphone-16-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/pvKZb60N/iphone-16-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/dC3dCvK/iphone-16-blk-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 60
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 Pro Max 512GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/tTKYk815/iphone-15-pro-max-nat-tit-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/v6vWDwcL/iphone-15-pro-max-nat-tit-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/yBW2HhSs/iphone-15-pro-max-nat-tit-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "512GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 Pro 256Gb",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/PZSpkWhf/1-1397x1397-jpg.webp",
                            ImageUrl2 = "https://i.ibb.co/1wG7rCG/2-1397x1397-jpg.webp",
                            ImageUrl3 = "https://i.ibb.co/m5bvhsGZ/3-1397x1397-jpg.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/tMmqrsW9/iphone-15-pnk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/WvBCVVbK/iphone-15-pnk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/pjX8MS0N/iphone-15-pnk-03-1600x1600.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 60
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 14 Pro Max 256Gb",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/fVWkjKYF/iphone-14-pro-gld-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/9mGhqbzV/iphone-14-pro-gld-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/0pmrSDt6/iphone-14-pro-gld-03-1600x1600.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 14 Pro 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://i.ibb.co/Q3rhMQgt/iphone-14-pro-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/Lz0Rpbb9/iphone-14-pro-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Spwq196/iphone-14-pro-blk-03-1600x1600.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S25 Ultra 12/512GB",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/gFQt2MQm/samsung-s25-ultra-s938-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/PZg3Ng33/samsung-s25-ultra-s938-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/6cZZqmKW/samsung-s25-ultra-s938-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.9",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S24 Ultra 12/512GB",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/jvJcfy66/samsung-s24-ultra-s928-yel-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/cctJFfrc/samsung-s24-ultra-s928-yel-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/ynX6bBr9/samsung-s24-ultra-s928-yel-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.8",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S23 Ultra 12/512Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/mr8bH5HH/samsung-s23ultra-lvndr-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/tptN7P5w/samsung-s23ultra-lvndr-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/HLxvRt6n/samsung-s23ultra-lvndr-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.8",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S24 8/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/Tqp0dqYm/samsung-s24-s921-grey-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/XfTJBvSS/samsung-s24-s921-grey-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/VYJKzw6X/samsung-s24-s921-grey-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.2",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy Fold7 12/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/Rpwfvdw5/samsung-fold7-sm-f966-blue-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/RT81xdJB/samsung-fold7-sm-f966-blue-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/1YRv5ZGn/samsung-fold7-sm-f966-blue-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "7.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy Flip7 12/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/1fKmXJSV/samsung-flip7-sm-f766-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/whBVYT9K/samsung-flip7-sm-f766-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/s9qMS1mM/samsung-flip7-sm-f766-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy A56 5G 8/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/ymjvTKMd/samsung-a56-a566-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/fYL8RbyK/samsung-a56-a566-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/XZzs5vBt/samsung-a56-a566-blk-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy A36 5G 8/256",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://i.ibb.co/G4wfFkBX/samsung-a36-a366-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/QjQ3fJ81/samsung-a36-a366-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/60WNZg1z/samsung-a36-a366-blk-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 9 Pro",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/jvfGNztX/Pixel-9-Pro-XL-hzl-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/1G26SF9Y/Pixel-9-Pro-XL-hzl-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Zv9Y733/Pixel-9-Pro-XL-hzl-03-1600x1600.webp",
                            Ram = "16GB",
                            Storage = "128GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 9",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/RpCn6Fw9/Pixel-9-obsd-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/bgp4fnjT/Pixel-9-obsd-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/hFYVbBs8/Pixel-9-obsd-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "128GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 8",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/pjsbPKqw/pixel-8-hzl-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/0VnjQGqQ/pixel-8-hzl-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/5XGFQJQH/pixel-8-hzl-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "128GB",
                            DisplaySize = "6.2",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 7a",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/NnCCMZbp/Google-Pixel-7a-wht-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/tM2PTH2T/Google-Pixel-7a-wht-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/7xVCTkgN/Google-Pixel-7a-wht-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "128GB",
                            DisplaySize = "6.1",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel Fold",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/tTZRwHtd/pixel-fold-obsd-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/jZ9cyW8M/pixel-fold-obsd-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Sw2qQ1g2/pixel-fold-obsd-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "7.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 6 Pro",
                            Manufacturer = "Google",
                            ImageUrl = "https://i.ibb.co/gZt7Br5c/google-pixel-6-pro-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/D615B21/google-pixel-6-pro-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/C5Xy9SpD/google-pixel-6-pro-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "128GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi 15 Ultra 16/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/q3Kk7LgX/xiaomi-15-ultra-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/ksnS2bxW/xiaomi-15-ultra-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Wv9R56XF/xiaomi-15-ultra-blk-03-1600x1600.webp",
                            Ram = "16GB",
                            Storage = "512GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi Redmi Note 14 Pro 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/ks9kmQtZ/redmi-note-14-pro-gld-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/pvMmBKJm/redmi-note-14-pro-gld-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/xSMv5vCb/redmi-note-14-pro-gld-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi Redmi Note 14 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/KxkTvWvb/redmi-note-14-blue-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/cXvsLfdY/redmi-note-14-blue-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Jjrq7Q2G/redmi-note-14-blue-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi 13T Pro 12/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/hJYnkndn/xiaomi-13t-pro-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/cc78Z9nv/xiaomi-13t-pro-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/twynxsqW/xiaomi-13t-pro-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.67",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "POCO F7 Pro 12/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/MxCnjHJQ/poco-f7-pro-slv-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/DfkQf23C/poco-f7-pro-slv-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/39MPSRbh/poco-f7-pro-slv-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "XIAOMI Redmi 15C 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://i.ibb.co/LDq82gBJ/redmi-15c-blue-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/5XqhqCmZ/redmi-15c-blue-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/sdBf5C22/redmi-15c-blue-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.71",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "Oneplus Ace 5 Racing 5G 12/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://i.ibb.co/qFLb4YDN/oneplus-ace5-racing-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/BHX546cS/oneplus-ace5-racing-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Q3FhGLhJ/oneplus-ace5-racing-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.74",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Oneplus Nord CE 4 Lite 5G 8/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://i.ibb.co/DDCWGpmJ/One-Plus-Nord-CE4-Lite-slv-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/bgm6f8Tb/One-Plus-Nord-CE4-Lite-slv-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/gZ4ZBgss/One-Plus-Nord-CE4-Lite-slv-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "OnePlus Nord 3 5G 16/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://i.ibb.co/fGzTJ2HT/Oneplus-Nord-3-gray-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/6Jg3ZwYH/Oneplus-Nord-3-gray-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Kjmfjqwz/Oneplus-Nord-3-gray-03-1600x1600.webp",
                            Ram = "16GB",
                            Storage = "256GB",
                            DisplaySize = "6.74",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "OnePlus 12 5G 12/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://i.ibb.co/ZRZM86k9/oneplus-12-blk-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/VYJbPp1B/oneplus-12-blk-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/TBsrCnCf/oneplus-12-blk-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.82",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Motorola Edge 60 Fusion 8/256Gb",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://i.ibb.co/4gDKgPJQ/moto-edge-60-fusion-slipstream-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/ksbwgDgs/moto-edge-60-fusion-slipstream-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/mV33ZT5R/moto-edge-60-fusion-slipstream-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "Motorola Edge 50 Fusion 8/256Gb",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://i.ibb.co/v5MpyP8/motorola-edge50-fusion-grn-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/RGJr6QNx/motorola-edge50-fusion-grn-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/tTxpc3fR/motorola-edge50-fusion-grn-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "MOTOROLA G05 4/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://i.ibb.co/Mkr85yhr/motorola-moto-g05-blue-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/N6tmv54R/motorola-moto-g05-blue-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/Wpd0ztmY/motorola-moto-g05-blue-03-1600x1600.webp",
                            Ram = "4GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "MOTOROLA G85 8/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://i.ibb.co/wZF3HH6n/Moto-G85-grn-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/0RrbCpMn/Moto-G85-grn-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/twr8qnmv/Moto-G85-grn-03-1600x1600.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Moto Edge 40 Neo 12/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://i.ibb.co/V0N04vhd/Moto-Edge-40-Neo-sea-01-1600x1600.webp",
                            ImageUrl2 = "https://i.ibb.co/0ps8CvH1/Moto-Edge-40-Neo-sea-02-1600x1600.webp",
                            ImageUrl3 = "https://i.ibb.co/602RSpC6/Moto-Edge-40-Neo-sea-03-1600x1600.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.55",
                            DisplayHz = 144
                        },
                    };
                    await context.Smartphones.AddRangeAsync(smartphones);
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}