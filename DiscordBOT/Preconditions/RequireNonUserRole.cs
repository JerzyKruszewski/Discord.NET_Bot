using System;
using System.Linq;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBOT.Configs;
using DiscordBOT.Miscellaneous;

namespace DiscordBOT.Preconditions
{
    public class RequireNonUserRole : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg((SocketGuild)context.Guild);

            SocketRole role = Methods.GetRoleByID((SocketGuild)context.Guild, guildCfg.UserRoleID);

            if (!(context.User as SocketGuildUser).Roles.Contains(role))
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Wywołujący jest już użytkownikiem"));
            }
        }
    }
}
