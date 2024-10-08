using Discord;
using Discord.WebSocket;
using DiscordBot.Commands;
using DiscordBot.Data;
using DiscordBot.Events;


namespace DiscordBot.Services
{
    public class DiscordBotService : IHostedService
    {
        private readonly IServiceProvider _serviceProvider;
        private readonly IConfiguration _configuration;
        private static DiscordSocketClient _client;
        private readonly ILogger<DiscordBotService> _logger;


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

        public async Task StartAsync(CancellationToken cancellationToken)
        {
            using (var scope = _serviceProvider.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<BotDBContext>();
                _client.Log += LogAsync;
                _client.Ready += ReadyAsync;
                _client.SlashCommandExecuted += SlashCommandHandler.HandleAsync;
                _client.UserJoined += GuildMemberEvents.UserJoinedAsync;
                await _client.LoginAsync(TokenType.Bot, _configuration.GetValue<string>("BOT_TOKEN"));
                await _client.StartAsync();
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
            var guild = _client.GetGuild(_configuration.GetValue<ulong>("DISCORD_SERVER"));
            var commands = new SlashCommandBuilder()
                .WithName("ping")
                .WithDescription("Replies with pong!");

            await guild.CreateApplicationCommandAsync(commands.Build());
        }

    }
}
