using System.Threading.Tasks;
using Discord.WebSocket;

namespace DiscordBot.Events
{
    public static class GuildMemberEvents
    {
        public static async Task UserJoinedAsync(SocketGuildUser user)
        {
            var channel = user.Guild.DefaultChannel;
            if (channel != null)
            {
                await channel.SendMessageAsync($"Welcome to the server, {user.Mention}!");
            }
        }
    }
}
