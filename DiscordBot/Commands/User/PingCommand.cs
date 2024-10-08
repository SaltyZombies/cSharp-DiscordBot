using Discord;
using Discord.Interactions;


namespace DiscordBot.Commands.User
{
    public partial class UserCommands : InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("ping", "Ping")]
        public async Task ping()
        {
            await RespondAsync("Pong!");
        }
    }
}