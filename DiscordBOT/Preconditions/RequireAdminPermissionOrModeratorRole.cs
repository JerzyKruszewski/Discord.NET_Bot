using System;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using DiscordBOT.Configs;
using DiscordBOT.Miscellaneous;

namespace DiscordBOT.Preconditions
{
    public class RequireAdminPermissionOrModeratorRole : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg((SocketGuild)context.Guild);

            IRole role = Methods.GetRoleByID((SocketGuild)context.Guild, guildCfg.ModeratorRoleID);

            if ((context.User as SocketGuildUser).Roles.Contains(role) || (context.User as SocketGuildUser).GuildPermissions.Administrator)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Nie masz uprawnień"));
            }
        }
    }
}
