using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using OAuthExample.Service.Clients;
using OAuthExample.Service.Options;
using OAuthExample.Service.Repositories;
using OAuthExample.Service.Services;

namespace OAuthExample.AppClient
{
    internal static class Program
    {
        /// <summary>
        ///  The main entry point for the application.
        /// </summary>
        [STAThread]
        static void Main()
        {
            // To customize application configuration such as set high DPI settings or default font,
            // see https://aka.ms/applicationconfiguration.
            ApplicationConfiguration.Initialize();

            var serviceProvider = BuildServiceProvider();
            var form = serviceProvider.GetRequiredService<Form1>();
            Application.Run(form);
        }

        private static IServiceProvider BuildServiceProvider()
        {
            var services = new ServiceCollection();
            var configuration = new ConfigurationBuilder()
                .SetBasePath(Directory.GetCurrentDirectory())
                .AddJsonFile("appsettings.json", optional: false)
                .AddJsonFile("appsettings.Development.json", optional: true)
                .Build();

            services.AddHttpClient();
            services.AddScoped<Form1>();
            services.AddSingleton(TimeProvider.System);
            services.AddScoped<IOAuthService, GoogleOAuthService>();
            services.AddScoped<IOAuthService, LineOAuthService>();
            services.AddScoped<IOAuthService, GithubOAuthService>();
            services.AddScoped<IOAuthService, MicrosoftOAuthService>();
            services.AddScoped<ILoginService, LoginService>();
            services.AddScoped<ILoginRepository, LoginRepository>();
            services.AddScoped<IStateManageService, StateManageService>();
            services.Configure<GoogleLoginOptions>(configuration.GetSection("GoogleLogin"));
            services.Configure<LineLoginOptions>(configuration.GetSection("LineLogin"));
            services.Configure<GithubLoginOptions>(configuration.GetSection("GithubLogin"));
            services.Configure<MicrosoftLoginOptions>(configuration.GetSection("MicrosoftLogin"));
            services.Configure<StateManageOptions>(configuration.GetSection("StateManage"));
            return services.BuildServiceProvider();
        }
    }
}