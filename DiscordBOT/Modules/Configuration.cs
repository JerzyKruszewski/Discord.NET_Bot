using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using DiscordBOT.Configs;
using DiscordBOT.Core;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Modules
{
    public class Configuration : InteractiveBase<SocketCommandContext>
    {
        [Command("Configuration")]
        [Alias("Config", "Cfg")]
        [RequireOwner]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.Administrator)]
        public async Task Cfg(IRole adminRole, IRole modRole, IRole userRole, IRole maleRole, IRole femaleRole, IRole punishmentRole,
            ITextChannel modChannel, ITextChannel generalChannel, ITextChannel statuteChannel, ITextChannel logChannel,
            ITextChannel inChannel, ITextChannel outChannel, ITextChannel touChannel, ITextChannel punishChannel,
            uint days = 0, uint warnDuration = 0)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            guildCfg.AdminRoleID = adminRole.Id;
            guildCfg.ModeratorRoleID = modRole.Id;
            guildCfg.UserRoleID = userRole.Id;
            guildCfg.MaleRoleID = maleRole.Id;
            guildCfg.FemaleRoleID = femaleRole.Id;
            guildCfg.PunishmentRoleID = punishmentRole.Id;

            guildCfg.ModeratorChannelID = modChannel.Id;
            guildCfg.GeneralChannelID = generalChannel.Id;
            guildCfg.StatuteChannelID = statuteChannel.Id;
            guildCfg.LogChannelID = logChannel.Id;
            guildCfg.InChannelID = inChannel.Id;
            guildCfg.OutChannelID = outChannel.Id;
            guildCfg.ToUChannelID = touChannel.Id;
            guildCfg.PunishmentChannelID = punishChannel.Id;

            guildCfg.DaysTillNextNicknameChange = days;
            guildCfg.WarnDuration = warnDuration;

            GuildsCfgs.Save();

            GuildCfg guildCfg2 = GuildsCfgs.GetGuildCfg(Context.Guild);

            await Context.Channel.SendMessageAsync($"{guildCfg2.GuildID}|{guildCfg2.UserRoleID}");
        }

        [Command("cfg check")]
        [RequireUserPermission(GuildPermission.Administrator)]
        public async Task CfgCheck()
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            await Context.Channel.SendMessageAsync($"{guildCfg.GuildID}|{guildCfg.UserRoleID}");
        }
    }
}
