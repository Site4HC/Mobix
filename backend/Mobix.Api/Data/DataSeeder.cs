using Mobix.Api.Models;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Builder; 
using Microsoft.Extensions.DependencyInjection; 

namespace Mobix.Api.Data
{
    public static class DataSeeder
    {
        public static async Task SeedDatabaseAsync(this IApplicationBuilder app)
        {
            using (var scope = app.ApplicationServices.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<ApplicationDbContext>();
                
                try
                {
                    await context.Database.EnsureCreatedAsync(); 
                }
                catch (Exception ex)
                {
                    Console.WriteLine($"Помилка при EnsureCreatedAsync (Імовірно, таблиці існують): {ex.Message}");
                }


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
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/iphone-17-pro-blue-01-1600x1600_tkixzf.webp", 
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/iphone-17-pro-blue-02-1600x1600_jsds6l.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/iphone-17-pro-blue-03-1600x1600_najoeb.webp",
                            Ram = "12GB", 
                            Storage = "256GB", 
                            DisplaySize = "6.9", 
                            DisplayHz = 120,
                        },
                        new Smartphone 
                        {   Name = "Apple iPhone 17 Pro 256GB", 
                            Manufacturer = "Apple", 
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105223/iphone-17-pro-orn-01-1600x1600_hn02hd.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-17-pro-orn-02-1600x1600_ja0pto.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-17-pro-orn-03-1600x1600_c9za7y.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120,
                        },
                        new Smartphone 
                        {   Name = "Apple iPhone 17 256GB", 
                            Manufacturer = "Apple", 
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/iphone-17-blk-011-1600x1600_efiavp.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103635/iphone-17-blk-02-1600x1600_veotgo.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/iphone-17-blk-03-1600x1600_ap0idm.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120,
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 Pro Max 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-16-pro-dsrt-01-1600x1600_faqn6c.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-16-pro-dsrt-02-1600x1600_vxc3rk.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-16-pro-dsrt-03-1600x1600_meblwq.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.9",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 Pro 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-16-pro-wht-01-1600x1600_jwuipb.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105197/iphone-16-pro-wht-02-1600x1600_nlnhnu.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105172/iphone-16-pro-wht-03-1600x1600_jsgcfl.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 16 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-16-blk-01-1600x1600_xyqmct.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-16-blk-02-1600x1600_qa4ner.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105221/iphone-16-blk-03-1600x1600_gttxqx.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 60
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 Pro Max 512GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105152/iphone-15-pro-max-nat-tit-01-1600x1600_tdamme.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105152/iphone-15-pro-max-nat-tit-02-1600x1600_jamrnz.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104976/iphone-15-pro-max-nat-tit-03-1600x1600_v9w62s.webp",
                            Ram = "8GB",
                            Storage = "512GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 Pro 256Gb",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104922/1-1397x1397.jpg_pkl6er.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104951/2-1397x1397.jpg_fdqlcr.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104922/3-1397x1397.jpg_hvxbx5.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 15 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-15-pnk-01-1600x1600_mmaymv.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-15-pnk-02-1600x1600_lrfvxy.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764105153/iphone-15-pnk-03-1600x1600_a6a14q.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 60
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 14 Pro Max 256Gb",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104861/iphone-14-pro-gld-01-1600x1600_dweqqe.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104839/iphone-14-pro-gld-02-1600x1600_tpooh1.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104839/iphone-14-pro-gld-03-1600x1600_marrl4.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Apple iPhone 14 Pro 256GB",
                            Manufacturer = "Apple",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104921/iphone-14-pro-blk-01-1600x1600_j1ato1.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104921/iphone-14-pro-blk-02-1600x1600_v6azr3.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104886/iphone-14-pro-blk-03-1600x1600_gmux6w.webp",
                            Ram = "6GB",
                            Storage = "256GB",
                            DisplaySize = "6.1",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S25 Ultra 12/512GB",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104838/samsung-s25-ultra-s938-blk-01-1600x1600_eqxzcr.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104838/samsung-s25-ultra-s938-blk-02-1600x1600_lgu199.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104838/samsung-s25-ultra-s938-blk-03-1600x1600_vaae1p.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.9",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S24 Ultra 12/512GB",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179610/samsung-s24-ultra-s928-yel-01-1600x1600_aw7x9l.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179640/samsung-s24-ultra-s928-yel-02-1600x1600_dozaad.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179623/samsung-s24-ultra-s928-yel-03-1600x1600_y0bcpa.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.8",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S23 Ultra 12/512Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104838/samsung-s23ultra-lvndr-01-1600x1600_v3y9ez.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104838/samsung-s23ultra-lvndr-02-1600x1600_hhyp07.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104837/samsung-s23ultra-lvndr-03-1600x1600_pom6pb.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.8",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy S24 8/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179547/samsung-s24-s921-grey-01-1600x1600_dkva8i.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179561/samsung-s24-s921-grey-02-1600x1600_sertut.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764179572/samsung-s24-s921-grey-03-1600x1600_chql82.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.2",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy Fold7 12/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104829/samsung-fold7-sm-f966-blue-01-1600x1600_uqfniu.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104827/samsung-fold7-sm-f966-blue-02-1600x1600_chxrpd.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104827/samsung-fold7-sm-f966-blue-03-1600x1600_sjv3qv.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "7.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy Flip7 12/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104827/samsung-flip7-sm-f766-blk-01-1600x1600_zi9khd.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104592/samsung-flip7-sm-f766-blk-02-1600x1600_xlyic7.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104567/samsung-flip7-sm-f766-blk-03-1600x1600_pvp3hy.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy A56 5G 8/256Gb",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104550/samsung-a56-a566-blk-01-1600x1600_br3cte.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104550/samsung-a56-a566-blk-02-1600x1600_ql1ktq.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104550/samsung-a56-a566-blk-03-1600x1600_e1npge.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Samsung Galaxy A36 5G 8/256",
                            Manufacturer = "Samsung",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104550/samsung-a36-a366-blk-01-1600x1600_eye42l.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104514/samsung-a36-a366-blk-02-1600x1600_bz6esw.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104490/samsung-a36-a366-blk-03-1600x1600_cpbc4v.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 9 Pro",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104461/Pixel-9-Pro-XL-hzl-01-1600x1600_prclyz.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104461/Pixel-9-Pro-XL-hzl-02-1600x1600_lmdgeh.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104462/Pixel-9-Pro-XL-hzl-03-1600x1600_ixv6sx.webp",
                            Ram = "16GB",
                            Storage = "128GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 9",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104462/Pixel-9-obsd-01-1600x1600_qjjhmt.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104462/Pixel-9-obsd-02-1600x1600_c55a6a.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104461/Pixel-9-obsd-03-1600x1600_kjwdwq.webp",
                            Ram = "12GB",
                            Storage = "128GB",
                            DisplaySize = "6.3",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 8",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104461/pixel-8-hzl-01-1600x1600_z18qed.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104461/pixel-8-hzl-02-1600x1600_fpr7lt.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104460/pixel-8-hzl-03-1600x1600_ytft6y.webp",
                            Ram = "8GB",
                            Storage = "128GB",
                            DisplaySize = "6.2",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 7a",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/Google-Pixel-7a-wht-01-1600x1600_t16pmi.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/Google-Pixel-7a-wht-02-1600x1600_l0crug.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/Google-Pixel-7a-wht-03-1600x1600_wbxav3.webp",
                            Ram = "8GB",
                            Storage = "128GB",
                            DisplaySize = "6.1",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel Fold",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/pixel-fold-obsd-01-1600x1600_m2khuc.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/pixel-fold-obsd-02-1600x1600_tf166a.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/pixel-fold-obsd-03-1600x1600_qpffjj.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "7.6",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Google Pixel 6 Pro",
                            Manufacturer = "Google",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104449/google-pixel-6-pro-blk-01-1600x1600_ygsm6d.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104448/google-pixel-6-pro-blk-02-1600x1600_qsf2nf.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104440/google-pixel-6-pro-blk-03-1600x1600_wnhzri.webp",
                            Ram = "12GB",
                            Storage = "128GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi 15 Ultra 16/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104440/xiaomi-15-ultra-blk-01-1600x1600_pmjpp9.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104439/xiaomi-15-ultra-blk-02-1600x1600_cucaxi.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104433/xiaomi-15-ultra-blk-03-1600x1600_reskhz.webp",
                            Ram = "16GB",
                            Storage = "512GB",
                            DisplaySize = "6.7",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi Redmi Note 14 Pro 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/redmi-note-14-pro-gld-01-1600x1600_trkkgu.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/redmi-note-14-pro-gld-02-1600x1600_g76qoo.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103636/redmi-note-14-pro-gld-03-1600x1600_jmkvsj.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi Redmi Note 14 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/redmi-note-14-blue-01-1600x1600_hwtajd.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/redmi-note-14-blue-02-1600x1600_oemqa0.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/redmi-note-14-blue-03-1600x1600_rkeipw.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Xiaomi 13T Pro 12/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/xiaomi-13t-pro-blk-01-1600x1600_h1v4xh.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/xiaomi-13t-pro-blk-02-1600x1600_u6pic4.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104428/xiaomi-13t-pro-blk-03-1600x1600_g82v6b.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.67",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "POCO F7 Pro 12/512GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/poco-f7-pro-slv-01-1600x1600_apk7ho.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104428/poco-f7-pro-slv-02-1600x1600_wfwcmu.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104428/poco-f7-pro-slv-03-1600x1600_xeajgp.webp",
                            Ram = "12GB",
                            Storage = "512GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "XIAOMI Redmi 15C 8/256GB",
                            Manufacturer = "Xiaomi",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104429/redmi-15c-blue-01-1600x1600_octbs1.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104428/redmi-15c-blue-02-1600x1600_d1yufq.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764104428/redmi-15c-blue-03-1600x1600_nmf0b6.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.71",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "Oneplus Ace 5 Racing 5G 12/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103701/oneplus-ace5-racing-blk-01-1600x1600_h9n2wl.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103676/oneplus-ace5-racing-blk-02-1600x1600_wvoncx.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103651/oneplus-ace5-racing-blk-03-1600x1600_lsa9z0.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.74",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Oneplus Nord CE 4 Lite 5G 8/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103651/OnePlus-Nord-CE4-Lite-slv-01-1600x1600_xezrpc.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103651/OnePlus-Nord-CE4-Lite-slv-02-1600x1600_ruzexl.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103651/OnePlus-Nord-CE4-Lite-slv-03-1600x1600_avf466.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "OnePlus Nord 3 5G 16/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103650/Oneplus-Nord-3-gray-01-1600x1600_pys6qs.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/Oneplus-Nord-3-gray-02-1600x1600_wmr101.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103650/Oneplus-Nord-3-gray-03-1600x1600_ypau7i.webp",
                            Ram = "16GB",
                            Storage = "256GB",
                            DisplaySize = "6.74",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "OnePlus 12 5G 12/256GB",
                            Manufacturer = "OnePlus",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103650/oneplus-12-blk-01-1600x1600_jdfw40.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103650/oneplus-12-blk-02-1600x1600_fsk2ex.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103644/oneplus-12-blk-03-1600x1600_vvrpv1.webp",
                            Ram = "12GB",
                            Storage = "256GB",
                            DisplaySize = "6.82",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Motorola Edge 60 Fusion 8/256Gb",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103644/moto-edge-60-fusion-slipstream-01-1600x1600_s3vghl.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/moto-edge-60-fusion-slipstream-02-1600x1600_xc61on.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/moto-edge-60-fusion-slipstream-03-1600x1600_ijwkne.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "Motorola Edge 50 Fusion 8/256Gb",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/motorola-edge50-fusion-grn-01-1600x1600_sl2dbi.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/motorola-edge50-fusion-grn-02-1600x1600_bulyb0.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103643/motorola-edge50-fusion-grn-03-1600x1600_ud7ws2.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.7",
                            DisplayHz = 144
                        },
                        new Smartphone
                        {
                            Name = "MOTOROLA G05 4/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/motorola-moto-g05-blue-01-1600x1600_rsulek.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103638/motorola-moto-g05-blue-02-1600x1600_n7qjsd.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/motorola-moto-g05-blue-03-1600x1600_nkadlu.webp",
                            Ram = "4GB",
                            Storage = "256GB",
                            DisplaySize = "6.6",
                            DisplayHz = 90
                        },
                        new Smartphone
                        {
                            Name = "MOTOROLA G85 8/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/Moto-G85-grn-01-1600x1600_uhttxm.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/Moto-G85-grn-02-1600x1600_etnfsm.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/Moto-G85-grn-03-1600x1600_tr0okw.webp",
                            Ram = "8GB",
                            Storage = "256GB",
                            DisplaySize = "6.67",
                            DisplayHz = 120
                        },
                        new Smartphone
                        {
                            Name = "Moto Edge 40 Neo 12/256GB",
                            Manufacturer = "Motorola",
                            ImageUrl = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/Moto-Edge-40-Neo-sea-01-1600x1600_lw7pyu.webp",
                            ImageUrl2 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103637/Moto-Edge-40-Neo-sea-02-1600x1600_sm9wyr.webp",
                            ImageUrl3 = "https://res.cloudinary.com/dffsrmdhb/image/upload/v1764103080/Moto-Edge-40-Neo-sea-03-1600x1600_eu2o80.webp",
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