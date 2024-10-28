using Maqalety.Code;
using MaqaletyCore;
using MaqaletyData;
using MaqaletyData.SqlEf;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.EntityFrameworkCore;

namespace Maqalety
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            builder.Services.AddControllersWithViews();

            // Use the configuration from the builder
            builder.Services.AddDbContext<Data.AppDbContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

            // If you have another context you want to use, register it accordingly.
            builder.Services.AddDbContext<AppDbContext>();

            // Configure Identity
            builder.Services.AddIdentity<IdentityUser, IdentityRole>(options =>
            {
                options.SignIn.RequireConfirmedAccount = true;
                options.SignIn.RequireConfirmedEmail = true;
            })
            .AddDefaultTokenProviders()
            .AddEntityFrameworkStores<Data.AppDbContext>();

            builder.Services.AddScoped<IEmailSender, EmailSender>();
            // Other service configurations
            builder.Services.AddScoped<IDataHelper<Category>, CategoryEntity>();
            builder.Services.AddScoped<IDataHelper<Author>, AuthorEntity>();
            builder.Services.AddScoped<IDataHelper<AuthorPost>, AuthorPostsEntity>();
            builder.Services.AddRazorPages();

            builder.Services.AddAuthorization(op =>
            {
                op.AddPolicy("User", p => p.RequireClaim("User", "User"));
                op.AddPolicy("Admin", p => p.RequireClaim("Admin", "Admin"));
            });

            builder.Services.AddCors(option =>
            {
                option.AddPolicy("MyPolicy", options =>
                {
                    options.AllowAnyOrigin().AllowAnyMethod().AllowAnyHeader();
                });
            });

            var app = builder.Build();

            // Middleware configuration
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();
            app.UseCors("MyPolicy");

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoint =>
            {
                endpoint.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}"
                );
                endpoint.MapRazorPages();
            });

            app.Run();
        }
    }
}
