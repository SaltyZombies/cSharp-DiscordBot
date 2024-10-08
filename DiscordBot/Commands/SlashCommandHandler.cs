using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBot.Commands
{
    public static class SlashCommandHandler
    {
        public static async Task HandleAsync(SocketSlashCommand command)
        {
            switch (command.Data.Name)
            {
                case "ping":
                    await command.RespondAsync("Pong!");
                    break;
                default:
                    await command.RespondAsync("Unknown command.");
                    break;
            }
        }
    }
}
