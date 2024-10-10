using Discord;
using Discord.Interactions;
using DiscordBot.Services;

namespace DiscordBot.Commands.User
{
    [Group("user", "User Commands")]
    public partial class UserCommands : InteractionModuleBase<InteractionContext>
    {
        private static ILogger<DiscordBotService> _logger = DiscordBotService.GetLogger();
    }
}
