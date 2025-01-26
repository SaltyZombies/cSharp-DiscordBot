using DotNetEnv;
using DiscordBot.Services;
using Microsoft.EntityFrameworkCore;
using DiscordBot.Data;

public class Program
{
    public static void Main(string[] args)
    {
        Env.Load();

        CreateHostBuilder(args).Build().Run();
    }

    public static IHostBuilder CreateHostBuilder(string[] args) =>
        Host.CreateDefaultBuilder(args)
            .ConfigureAppConfiguration((hostingContext, config) =>
             {
                 config.AddEnvironmentVariables();
             })
            .ConfigureServices((hostContext, services) =>
            {
                services.AddDbContext<BotDbContext>(options =>
                {
                    options.UseMySql(
                        Environment.GetEnvironmentVariable("MYSQL_CONNECTION_STRING"),
                        new MySqlServerVersion(new Version(8, 0, 32)) // Adjust MySQL version accordingly
                    );
                });
                services.AddSingleton<DiscordBotService>();
                services.AddHostedService(provider => provider.GetRequiredService<DiscordBotService>());
            });
}
