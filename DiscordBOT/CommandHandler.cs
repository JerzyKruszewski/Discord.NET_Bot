using System;
using System.Threading.Tasks;
using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using DiscordBOT.Core;
using DiscordBOT.Core.LevelingSystem;
using DiscordBOT.Core.Objects;
using DiscordBOT.Core.Logging;

namespace DiscordBOT
{
    class CommandHandler
    {
        DiscordSocketClient _client;
        CommandService _commands;
        IServiceProvider _service;

        public async Task InitializeAsync(DiscordSocketClient client)
        {
            _client = client;

            (_service as IDisposable)?.Dispose();
            _service = new ServiceCollection()
                .AddSingleton(client)
                .AddSingleton(new InteractiveService(_client))
                .BuildServiceProvider();

            var cmdConfig = new CommandServiceConfig
            {
                DefaultRunMode = RunMode.Async
            };

            (_commands as IDisposable)?.Dispose();
            _commands = new CommandService(cmdConfig);
            await _commands.AddModulesAsync(Assembly.GetEntryAssembly(), _service);

            _client.Disconnected += EventHandler.BotDisconnected;
            _client.Connected += EventHandler.BotConnected;
            _client.UserUpdated += EventHandler.UserUpdated;
            _client.MessageReceived += HandleCommandAsync;
            _client.UserBanned += EventHandler.UserBanned;
        }

        private async Task HandleCommandAsync(SocketMessage s)
        {
            try
            {
                var msg = s as SocketUserMessage;
                if (msg == null) return;
                var context = new SocketCommandContext(_client, msg);
                UserExpMute author = UsersExpMute.GetExpMute(msg.Author.Id); 

                if (author.IsMuted)
                {
                    await msg.DeleteAsync();
                    return;
                }

                //Leveling up
                Leveling.UserSendMessage(context.User);

                int argPos = 0;
                if (msg.HasStringPrefix(Config.bot.cmdPrefix, ref argPos)
                    || msg.HasMentionPrefix(_client.CurrentUser, ref argPos))
                {
                    var result = await _commands.ExecuteAsync(context, argPos, _service);
                    if (!result.IsSuccess && result.Error != CommandError.UnknownCommand)
                    {
                        Console.WriteLine(result.ErrorReason);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex);
                throw;
            }         
        }
    }
}
