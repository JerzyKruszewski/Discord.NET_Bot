namespace DiscordBOT.Configs
{
    public class GuildCfg
    {
        public ulong GuildID { get; set; }

        //Users
        public ulong GuildOwnerID { get; set; }

        //Roles
        public ulong AdminRoleID { get; set; } = 0;

        public ulong ModeratorRoleID { get; set; } = 0;

        public ulong UserRoleID { get; set; } = 0;

        public ulong MaleRoleID { get; set; } = 0;

        public ulong FemaleRoleID { get; set; } = 0;

        public ulong PunishmentRoleID { get; set; } = 0;

        //Channels
        public ulong ModeratorChannelID { get; set; } = 0;

        public ulong GeneralChannelID { get; set; } = 0;

        public ulong StatuteChannelID { get; set; } = 0;

        public ulong LogChannelID { get; set; } = 0;

        public ulong InChannelID { get; set; } = 0;

        public ulong OutChannelID { get; set; } = 0;

        public ulong ToUChannelID { get; set; } = 0;

        public ulong PunishmentChannelID { get; set; } = 0;

        //Params
        public uint DaysTillNextNicknameChange { get; set; } = 0;

        public uint WarnDuration { get; set; } = 0;

        //Messages
    }
}
