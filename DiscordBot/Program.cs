using DotNetEnv;
using DiscordBot.Services;
using Microsoft.EntityFrameworkCore;
using MongoDB.Driver;
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
                services.AddDbContext<BotDBContext>(options =>
                    options.UseMongoDB(
                        new MongoClient(Environment.GetEnvironmentVariable("MONGODB_URI") ?? hostContext.Configuration.GetSection("MongoSettings").GetValue<string>("ConnectionString")),
                        Environment.GetEnvironmentVariable("MONGODB_DB") ?? hostContext.Configuration.GetSection("MongoSettings").GetValue<string>("DatabaseName") ?? throw new InvalidOperationException("DatabaseName is not configured.")
                    ));
                services.AddSingleton<DiscordBotService>();
                services.AddHostedService(provider => provider.GetRequiredService<DiscordBotService>());
            });
}
