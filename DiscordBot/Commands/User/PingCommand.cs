using Discord;
using Discord.Interactions;
using ZstdSharp.Unsafe;


namespace DiscordBot.Commands.User
{
    public partial class UserCommands : InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("ping", "Ping the system")]
        public async Task Ping()
        {
            _logger.LogInformation($"{Context.User.Username} has attempted to ping us from {Context.Guild.Id}");
            await RespondAsync("Pong!");
        }
    }
}