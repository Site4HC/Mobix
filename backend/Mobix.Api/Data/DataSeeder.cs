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
                        new Store { Name = "Allo", BaseUrl = "https://allo.ua/" },
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
                        new Smartphone { Name = "Apple iPhone 16 Pro Max 256GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/i/p/iphone_16_pro_max_black_titanium_pdp_image_position_1__ce-ww_1.webp" },
                        new Smartphone { Name = "Apple iPhone 16 Pro 256GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/i/p/iphone_16_pro_max_black_titanium_pdp_image_position_1__ce-ww_1.webp" },
                        new Smartphone { Name = "Apple iPhone 16 256GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/i/p/iphone_16_black_pdp_image_position_1__ce-ww1725952011.webp" },
                        new Smartphone { Name = "Apple iPhone 15 Pro Max 512GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/7753358125914714.jpg" },
                        new Smartphone { Name = "Apple iPhone 15 Pro 256Gb", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/7753358125914714.jpg" },
                        new Smartphone { Name = "Apple iPhone 15 256GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/i/p/iphone_15_plus_black_pdp_image_position-1__ww-en.webp" },
                        new Smartphone { Name = "Apple iPhone 14 Pro Max 256Gb", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/6488294618051687.webp" },
                        new Smartphone { Name = "Apple iPhone 14 Pro 256GB", Manufacturer = "Apple", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/w/w/wwen_iphone14_q422_starlight_pdp_image_position-1a_1.webp" },

                        new Smartphone { Name = "Samsung Galaxy S25 Ultra 12/512GB", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/s/m/sm-s938_galaxys25ultra_front_titaniumblack_241107_result_1.webp" },
                        new Smartphone { Name = "Samsung Galaxy S24 Ultra 12/512GB", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/s/m/sm-s928_galaxys24ultra_front_titaniumblack_231109_result.jpg" },
                        new Smartphone { Name = "Samsung Galaxy S23 Ultra 12/512Gb", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/6287384621088378.webp" },
                        new Smartphone { Name = "Samsung Galaxy S24 8/256Gb", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/s/m/sm-s921_galaxys24_front_onyxblack_231110_1.jpg" },
                        new Smartphone { Name = "Samsung Galaxy Fold7 12/256Gb", Manufacturer = "Samsung", ImageUrl = "https://files.foxtrot.com.ua/PhotoNew/img_0_60_11758_0_1_S3HkhB.webp" },
                        new Smartphone { Name = "Samsung Galaxy Flip7 12/256Gb", Manufacturer = "Samsung", ImageUrl = "https://scdn.comfy.ua/89fc351a-22e7-41ee-8321-f8a9356ca351/https://cdn.comfy.ua/media/catalog/product/f/i/file_2705_3.jpg/f_auto" },
                        new Smartphone { Name = "Samsung Galaxy A56 5G 8/256Gb", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/s/m/sm-a566_galaxy_a56_5g_awesome_graphite_front_result_2_1.webp" },
                        new Smartphone { Name = "Samsung Galaxy A36 5G 8/256", Manufacturer = "Samsung", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/2000x2000/af097278c5db4767b0fe9bb92fe21690/s/m/sm-a366_galaxy_a36_5g_awesome_white_front_result_2.webp" },

                        new Smartphone { Name = "Google Pixel 9 Pro", Manufacturer = "Google", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/6255229121004377.webp" },
                        new Smartphone { Name = "Google Pixel 9", Manufacturer = "Google", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/6443888821603420.webp" },
                        new Smartphone { Name = "Google Pixel 8", Manufacturer = "Google", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/7190800223758176.webp" },
                        new Smartphone { Name = "Google Pixel 7a", Manufacturer = "Google", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/7099940813617817.webp" },
                        new Smartphone { Name = "Google Pixel Fold", Manufacturer = "Google", ImageUrl = "https://files.foxtrot.com.ua/PhotoNew/img_0_60_11865_0_1_LRZUNP.webp" },
                        new Smartphone { Name = "Google Pixel 6 Pro", Manufacturer = "Google", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/4488184316341832.jpg" },

                        new Smartphone { Name = "Xiaomi 15 Ultra 16/512GB", Manufacturer = "Xiaomi", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/2/123123123_20.webp" },
                        new Smartphone { Name = "Xiaomi Redmi Note 14 Pro 8/256GB", Manufacturer = "Xiaomi", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/2/124124124_6.webp" },
                        new Smartphone { Name = "Xiaomi Redmi Note 14 8/256GB", Manufacturer = "Xiaomi", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/_/o/_o7-black-front-r1-wallpaper_result.webp" },
                        new Smartphone { Name = "Xiaomi 13T Pro 12/512GB", Manufacturer = "Xiaomi", ImageUrl = "https://content.rozetka.com.ua/goods/images/big/499580885.jpg" },
                        new Smartphone { Name = "POCO F7 Pro 12/512GB", Manufacturer = "Xiaomi", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/2/4/247237.webp" },
                        new Smartphone { Name = "XIAOMI Redmi 15C 8/256GB", Manufacturer = "Xiaomi", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/2/123123_24.webp" },

                        new Smartphone { Name = "Oneplus Ace 5 Racing 5G 12/256GB", Manufacturer = "OnePlus", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/7656219525632210.webp" },
                        new Smartphone { Name = "Oneplus Nord CE 4 Lite 5G 8/256GB", Manufacturer = "OnePlus", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/5630100219075209.webp" },
                        new Smartphone { Name = "OnePlus Nord 3 5G 16/256GB", Manufacturer = "OnePlus", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/import/3928760514855134.webp" },
                        new Smartphone { Name = "OnePlus 12 5G 12/256GB", Manufacturer = "OnePlus", ImageUrl = "https://i.citrus.world/imgcache/size_800/uploads/shop/1/7/1760422454-777355-01.webp" },

                        new Smartphone { Name = "Motorola Edge 60 Fusion 8/256Gb", Manufacturer = "Motorola", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/5/156527-1600-auto_result.webp" },
                        new Smartphone { Name = "Motorola Edge 50 Fusion 8/256Gb", Manufacturer = "Motorola", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/4/14141414141414_2_1_3.webp" },
                        new Smartphone { Name = "MOTOROLA G05 4/256GB", Manufacturer = "Motorola", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/3/4/347347347_3.webp" },
                        new Smartphone { Name = "MOTOROLA G85 8/256GB", Manufacturer = "Motorola", ImageUrl = "https://files.foxtrot.com.ua/PhotoNew/img_0_60_10350_0_1_MdFYQX.webp" },
                        new Smartphone { Name = "Moto Edge 40 Neo 12/256GB", Manufacturer = "Motorola", ImageUrl = "https://i.allo.ua/media/catalog/product/cache/3/image/710x600/602f0fa2c1f0d1ba5e241f914e856ff9/1/2/123123123123_22_3.jpg" },
                    };
                    await context.Smartphones.AddRangeAsync(smartphones);
                }
                
                await context.SaveChangesAsync();
            }
        }
    }
}