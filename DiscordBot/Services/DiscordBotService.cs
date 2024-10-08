using System.Runtime.InteropServices;
using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Data;
using DiscordBot.Events;
using DiscordBot.Services;


namespace DiscordBot.Services
{
    public class DiscordBotService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private static DiscordSocketClient _client;
        public readonly ILogger<DiscordBotService> _logger;
        private InteractionService _commands;


        public DiscordBotService(IServiceProvider serviceProvider, IConfiguration configuration, ILogger<DiscordBotService> logger)
        {
            _serviceProvider = serviceProvider;
            _configuration = configuration;
            _client = new DiscordSocketClient(new DiscordSocketConfig
            {
                GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers
            });
            _logger = logger;
        }

        private ServiceProvider ConfigureServices()
        {
            return new ServiceCollection()
                .AddSingleton(_configuration)
                .AddSingleton(_client)
                .AddSingleton(x => new InteractionService(_client))
                .AddSingleton<CommandHandler>()
                .BuildServiceProvider();
        }

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            try
            {
                DiscordSocketConfig socketConfig = new DiscordSocketConfig
                {
                    GatewayIntents = GatewayIntents.AllUnprivileged | GatewayIntents.GuildMembers | GatewayIntents.GuildBans
                };

                _logger.LogInformation("Starting Discord Bot");

                _client = new DiscordSocketClient(socketConfig);

                // Attach the log event
                _client.Log += LogAsync;
                using (var services = ConfigureServices())
                {
                    _commands = services.GetRequiredService<InteractionService>();
                    _commands.Log += LogAsync;
                    _client.Ready += ReadyAsync;
                    _client.UserJoined += GuildMemberEvents.UserJoinedAsync;

                    await _client.LoginAsync(TokenType.Bot, _configuration.GetValue<string>("BOT_TOKEN"));
                    await _client.StartAsync();
                    await services.GetRequiredService<CommandHandler>().InitializeAsync();
                }
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while starting the Discord Bot.");
            }
        }

        public async Task StopAsync(CancellationToken cancellationToken)
        {
            // Clean up your Discord.Net bot here
            await _client.StopAsync();
        }

        // Very Generic Log Handler
        private Task LogAsync(LogMessage msg)
        {
            switch (msg.Severity)
            {
                case LogSeverity.Critical:
                    _logger.LogCritical(msg.ToString());
                    break;
                case LogSeverity.Error:
                    _logger.LogError(msg.ToString());
                    break;
                case LogSeverity.Warning:
                    _logger.LogWarning(msg.ToString());
                    break;
                case LogSeverity.Info:
                    _logger.LogInformation(msg.ToString());
                    break;
                case LogSeverity.Verbose:
                    _logger.LogDebug(msg.ToString());
                    break;
                case LogSeverity.Debug:
                    _logger.LogDebug(msg.ToString());
                    break;
                default:
                    _logger.LogInformation(msg.ToString());
                    break;
            }
            return Task.CompletedTask;
        }

        private async Task ReadyAsync()
        {

            var testServer = Convert.ToUInt64(Environment.GetEnvironmentVariable("DISCORD_SERVER"));

            if (testServer != 0)
                {
                    _logger.LogInformation($"In Debug Mode, Adding commands to {testServer}");
                    await _commands.RegisterCommandsToGuildAsync(testServer);
                }
                else
                    await _commands.RegisterCommandsGloballyAsync(true);
        }
    }
}
