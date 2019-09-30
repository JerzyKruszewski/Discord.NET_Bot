using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBOT.Configs;
using DiscordBOT.Miscellaneous;

namespace DiscordBOT.Preconditions
{
    public class RequireToUAcceptanceChannel : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg((SocketGuild)context.Guild);

            SocketGuildChannel channel = Methods.GetTextChannelByID((SocketGuild)context.Guild, guildCfg.ToUChannelID);

            if (context.Channel == channel)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Niewłaściwy kanał."));
            }
        }
    }
}
