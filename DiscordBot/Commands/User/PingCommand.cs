using Discord;
using Discord.Interactions;


namespace DiscordBot.Commands.User
{
    public partial class UserCommands : InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("ping", "Ping the system")]
        public async Task Ping()
        {
            Console.WriteLine("Pong!");
            await RespondAsync("Pong!");
        }
    }
}