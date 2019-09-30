using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.WebSocket;

namespace DiscordBOT
{
    internal class Program
    {
        private DiscordSocketClient _client;
        private CommandHandler _handler;

        private static void Main()
        => new Program().RunBotAsync().GetAwaiter().GetResult();

        public async Task RunBotAsync()
        {
            try
            {
                if (Config.bot.token == "" || Config.bot.token == null) return;

                _client?.Dispose();
                _client = new DiscordSocketClient(new DiscordSocketConfig
                {
                    LogLevel = LogSeverity.Verbose,
                });

                await InitializationClient();
                await InitializationLogs();

                ConsoleInput();
                await Task.Delay(-1);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"ERROR: {ex.Message}");
            }
        }

        private async Task InitializationClient()
        {
            _client.ReactionAdded += OnReactionAdded;
            await LoginAsync();
            await HandlerInitialize();
        }

        private async Task LoginAsync()
        {
            await _client.LoginAsync(TokenType.Bot, Config.bot.token);
            await _client.StartAsync();
        }

        private async Task HandlerInitialize()
        {
            _handler = new CommandHandler();
            await _handler.InitializeAsync(_client);
        }

        private Task InitializationLogs()
        {
            _client.Log += BotLog;

            return Task.CompletedTask;
        }

        private async Task ConsoleInput()
        {
            string input = "";

            while (input.Trim().ToLower() != "block")
            {
                input = Console.ReadLine();
                if (input.Trim().ToLower() == "msg")
                {
                    ConsoleSendMessage();
                }
                if (input.Trim().ToLower() == "img")
                {
                    ConsoleSendFile();
                }
            }
        }

        private async void ConsoleSendMessage()
        {
            Console.WriteLine("Select the Guild: ");
            var guild = GetSelectedGuild(_client.Guilds);
            var TextChannel = GetSelectedTextChannel(guild.TextChannels);
            var msg = String.Empty;
            while (msg.Trim() == String.Empty)
            {
                Console.WriteLine("Your message:");
                msg = Console.ReadLine();
            }

            await TextChannel.SendMessageAsync(msg);
        }

        private SocketGuild GetSelectedGuild(IEnumerable<SocketGuild> guilds)
        {
            var socketGuilds = guilds.ToList();
            var maxIndex = guilds.Count() - 1;
            for (var i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} - {socketGuilds[i].Name} ({socketGuilds[i].Id})");
            }

            var selectedIndex = -1;
            bool success = false;
            while (!success || selectedIndex < 0 || selectedIndex > maxIndex)
            {
                success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);

                if (!success || selectedIndex < 0 || selectedIndex > maxIndex)
                {
                    Console.WriteLine("Invalid Index! Try Again!");
                }
            }

            return socketGuilds[selectedIndex];
        }

        private SocketTextChannel GetSelectedTextChannel(IEnumerable<SocketTextChannel> textChannels)
        {
            var socketTextChannels = textChannels.ToList();
            var maxIndex = textChannels.Count() - 1;
            for (var i = 0; i <= maxIndex; i++)
            {
                Console.WriteLine($"{i} - {socketTextChannels[i].Name} ({socketTextChannels[i].Id})");
            }

            var selectedIndex = -1;
            bool success = false;
            while (!success || selectedIndex < 0 || selectedIndex > maxIndex)
            {
                success = int.TryParse(Console.ReadLine().Trim(), out selectedIndex);

                if (!success || selectedIndex < 0 || selectedIndex > maxIndex)
                {
                    Console.WriteLine("Invalid Index! Try Again!");
                }
            }

            return socketTextChannels[selectedIndex];
        }

        private async void ConsoleSendFile()
        {
            Console.WriteLine("Select the Guild: ");
            var guild = GetSelectedGuild(_client.Guilds);
            var TextChannel = GetSelectedTextChannel(guild.TextChannels);
            var fileName = String.Empty;
            while (fileName.Trim() == String.Empty)
            {
                Console.WriteLine("Which one:");
                fileName = Console.ReadLine();
            }

            await TextChannel.SendFileAsync($"Resources/Images/{fileName}", "");
        }

        private async Task OnReactionAdded(Cacheable<IUserMessage, ulong> cache, ISocketMessageChannel channel, SocketReaction reaction)
        {
            if (reaction.MessageId == Global.MessageIDToTrack)
            {
                if (reaction.Emote.Name == "😂")
                {
                    await channel.SendMessageAsync("Done, and done!");
                    IGuildUser user = reaction.User.Value as IGuildUser;
                    IRole role = user.Guild.Roles.FirstOrDefault(x => x.Id == 475000715329142803); //kalosz
                    await user.AddRoleAsync(role);
                }
            }
        }

        private Task BotLog(LogMessage msg)
        {
            Console.WriteLine(msg.Message);
            return Task.CompletedTask;
        }
    }
}