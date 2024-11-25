using OAuthExample.Service;
using OAuthExample.Service.Options;
using OAuthExample.Web.Repositories;
using OAuthExample.Web.Services;

namespace OAuthExample.Web
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);
            var configuration = builder.Configuration;

            builder.Services.AddControllersWithViews();

            builder.Services.AddHttpClient();
            builder.Services.AddScoped<IOAuthService, GoogleOAuthService>();
            builder.Services.AddScoped<IOAuthService, LineOAuthService>();
            builder.Services.AddScoped<ILoginService, LoginService>();
            builder.Services.AddScoped<ILoginRepository, LoginRepository>();
            builder.Services.Configure<GoogleLoginOptions>(configuration.GetSection("GoogleLogin"));
            builder.Services.Configure<LineLoginOptions>(configuration.GetSection("LineLogin"));

            builder.Services.AddAuthentication("LoginAuth")
                .AddCookie("LoginAuth", options => { options.LoginPath = "/Home"; });

            var app = builder.Build();

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
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
