using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Photography.Data;
using Photography.Models;

namespace Photography
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Add services to the container.
            var connectionString = builder.Configuration.GetConnectionString("DefaultConnection") ?? throw new InvalidOperationException("Connection string 'DefaultConnection' not found.");
            builder.Services.AddDbContext<ApplicationDbContext>(options =>
                options.UseSqlServer(connectionString));
            builder.Services.AddDatabaseDeveloperPageExceptionFilter();

            builder.Services.AddDefaultIdentity<User>(options => options.SignIn.RequireConfirmedAccount = false)
                .AddRoles<IdentityRole>()
                .AddEntityFrameworkStores<ApplicationDbContext>();
            builder.Services.AddControllersWithViews();


            // Enable Google Auth
            builder.Services.AddAuthentication()
                .AddGoogle(options =>
                {
                    // Access Google Auth section of appsettings.Development.json, therefore, 'googleAuth' will store the ClientID & Client Secret
                    IConfigurationSection googleAuth = builder.Configuration.GetSection("Authentication:Google");

                    // Read Google API Key values from config
                    options.ClientId = googleAuth["ClientId"];
                    options.ClientSecret = googleAuth["ClientSecret"];
                });


            // Adding a new dependency so controllers can read configuration values
            builder.Services.AddSingleton<IConfiguration>(builder.Configuration);

            // Add sessions to our application
            builder.Services.AddSession(options => {
                options.IdleTimeout = TimeSpan.FromMinutes(5);
                options.Cookie.HttpOnly = true;
                options.Cookie.IsEssential = true;
            });


            var app = builder.Build();

            //Use our session
            app.UseSession();


            // Configure the HTTP request pipeline.
            if (app.Environment.IsDevelopment())
            {
                app.UseMigrationsEndPoint();
            }
            else
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            RouteConfig.ConfigureRoutes(app);
            app.MapRazorPages();

            app.Run();
        }
    }
}