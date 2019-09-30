using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Rest;
using DiscordBOT.Configs;
using DiscordBOT.Miscellaneous;
using DiscordBOT.Core;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core.Logging
{
    public class ChannelMonitoring
    {
        public static async void MonitorChannel(SocketCommandContext context)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(context.Guild);
            ISocketMessageChannel logChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(context.Guild, guildCfg.LogChannelID);

            await logChannel.SendMessageAsync($"{context.Message.CreatedAt} | {context.Message.Author} | {context.Message.Content}");
        }
    }
}
