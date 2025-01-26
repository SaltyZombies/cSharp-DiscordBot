using System.Diagnostics;
using Discord;
using Discord.Interactions;



namespace DiscordBot.Commands.User
{
    public partial class UserCommands : InteractionModuleBase<InteractionContext>
    {
        [SlashCommand("ping", "Ping the system")]
        public async Task Ping()
        {
            _logger.LogInformation($"{Context.User.Username} has attempted to ping us from {Context.Guild.Id}");
            var stopwatch = new Stopwatch();
            stopwatch.Start();
            
            var httpPing = 0;
            using (var httpClient = new HttpClient())
            {
                var response = await httpClient.GetAsync("https://discord.com");
                stopwatch.Stop();
                httpPing = (int)stopwatch.ElapsedMilliseconds;
                stopwatch.Reset();
            }
            
            await RespondAsync($"HTTP Ping: {httpPing}ms");
        }
    }
}
