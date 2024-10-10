using Discord;
using Discord.Interactions;
using Discord.WebSocket;
using System.Reflection;


namespace DiscordBot.Services
{
    public class CommandHandler
    {
        private readonly DiscordSocketClient _client;
        private readonly InteractionService _commands;
        private readonly IServiceProvider _services;
        private static ILogger<DiscordBotService> _logger;

        public CommandHandler(DiscordSocketClient client, InteractionService commands, IServiceProvider services)
        {
            _client = client;
            _commands = commands;
            _services = services;
            _logger = DiscordBotService.GetLogger();
        }

        public async Task InitializeAsync()
        {
            try
            {
                await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _services);
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding modules to the command service.");
            }

            try
            {
                _client.InteractionCreated += HandleInteraction;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding the InteractionCreated event handler.");
            }

            try
            {
                _commands.SlashCommandExecuted += SlashCommandExecuted;
                _commands.ComponentCommandExecuted += ComponentCommandExecuted;
            }
            catch (Exception ex)
            {
                _logger.LogCritical(ex, "An error occurred while adding the command execution event handlers.");
            }
        }

        private Task ComponentCommandExecuted(ComponentCommandInfo arg1, IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {

                _logger.LogWarning($"Component command failed: {(arg1 != null ? arg1.Name : "BlankName")} with error: {arg3.ErrorReason ?? "BlankReason"}");

                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        // Handle error
                        break;
                    case InteractionCommandError.UnknownCommand:
                        // Handle error
                        break;
                    case InteractionCommandError.BadArgs:
                        // Handle error
                        break;
                    case InteractionCommandError.Exception:
                        // Handle error
                        break;
                    case InteractionCommandError.Unsuccessful:
                        // Handle error
                        break;
                    default:
                        break;
                }
            }
            return Task.CompletedTask;
        }

        private async Task SlashCommandExecuted(SlashCommandInfo arg1, IInteractionContext arg2, IResult arg3)
        {
            if (!arg3.IsSuccess)
            {
                _logger.LogWarning($"Slash command failed: {(arg1 != null ? arg1.Name : "BlankName")} with error: {arg3.ErrorReason ?? "BlankReason"}");

                switch (arg3.Error)
                {
                    case InteractionCommandError.UnmetPrecondition:
                        await arg2.Interaction.RespondAsync(
                            text: $"{arg2.User.Mention}, You do not have permission to use this command.",
                            ephemeral: true);
                        break;
                    case InteractionCommandError.UnknownCommand:
                        await arg2.Interaction.RespondAsync(
                            text: $"{arg2.User.Mention}, The command you are trying to use does not exist.",
                            ephemeral: true);
                        break;
                    case InteractionCommandError.BadArgs:
                        await arg2.Interaction.RespondAsync(
                            text: $"{arg2.User.Mention}, The arguments you provided are invalid.",
                            ephemeral: true);
                        break;
                    case InteractionCommandError.Exception:
                        await arg2.Interaction.RespondAsync(
                            text: $"{arg2.User.Mention}, An error occurred while executing the command.",
                            ephemeral: true);
                        break;
                    case InteractionCommandError.Unsuccessful:
                        await arg2.Interaction.RespondAsync(
                            text: $"{arg2.User.Mention}, The command was not successful.",
                            ephemeral: true);
                        break;
                    default:
                        break;
                }
            }
        }

        private async Task HandleInteraction(SocketInteraction arg)
        {
            try
            {
                var ctx = new InteractionContext(_client, arg);
                await _commands.ExecuteCommandAsync(ctx, _services);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while executing an interaction command.");
                if (arg.Type == InteractionType.ApplicationCommand)
                {
                    await arg.GetOriginalResponseAsync().ContinueWith(async (msg) => await msg.Result.DeleteAsync());
                }
            }
        }
    }
}
