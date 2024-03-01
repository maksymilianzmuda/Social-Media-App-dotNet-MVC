using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using SocialMediaApp.Data;
using SocialMediaApp.Helpers;
using SocialMediaApp.Interfaces;
using SocialMediaApp.Models;
using SocialMediaApp.Repository;
using SocialMediaApp.Services;

namespace SocialMediaApp
{
    public class Program
    {
        
        public static void Main(string[] args)
        {
               



                var builder = WebApplication.CreateBuilder(args);

                // Add services to the container.
                builder.Services.AddControllersWithViews();
                builder.Services.AddScoped<IClubRepository, ClubRepository>();
                builder.Services.AddScoped<IRaceRepository, RaceRepository>();
                builder.Services.Configure<CloudinarySettings>(builder.Configuration.GetSection("Cloudinary"));
                builder.Services.AddScoped<IPhotoService, PhotoService>();
                builder.Services.AddDbContext<ApplicationDbContext>(options =>
                {
                    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection"));
                });
                builder.Services.AddIdentity<AppUser, IdentityRole>()
                    .AddEntityFrameworkStores<ApplicationDbContext>();
                builder.Services.AddMemoryCache();
                builder.Services.AddSession();
                builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                    .AddCookie();


                var app = builder.Build();
                if (args.Length == 1 && args[0].ToLower() == "seeddata")
                {
                Task task = Seed.SeedUsersAndRolesAsync(app);
                    //Seed.SeedData(app);
                }

                // Configure the HTTP request pipeline.
                if (!app.Environment.IsDevelopment())
                {
                    app.UseExceptionHandler("/Home/Error");
                    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                    app.UseHsts();
                }

                app.UseHttpsRedirection();
                app.UseStaticFiles();

                app.UseRouting();

                app.UseAuthorization();

                app.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");

                app.Run();
                  
        }

    }
}