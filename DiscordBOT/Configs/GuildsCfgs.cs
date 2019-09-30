using System.Collections.Generic;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Storage;

namespace DiscordBOT.Configs
{
    public class GuildsCfgs
    {
        private static List<GuildCfg> guildConfigs;

        private static string filepath = "Resources/Config/GuildConfigs";

        static GuildsCfgs()
        {
            if (JsonStorage.FileExist(filepath))
            {
                guildConfigs = JsonStorage.RestoreObject<List<GuildCfg>>(filepath);
            }
            else
            {
                guildConfigs = new List<GuildCfg>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(guildConfigs, filepath);
        }

        public static GuildCfg GetGuildCfg(SocketGuild guild)
        {
            return GetOrCreateGuildCfg(guild.Id, guild.OwnerId);
        }

        private static GuildCfg GetOrCreateGuildCfg(ulong id, ulong ownerID)
        {
            var result = from g in guildConfigs
                         where g.GuildID == id
                         select g;

            GuildCfg guildCfg = result.FirstOrDefault();

            if (guildCfg == null)
            {
                guildCfg = CreateGuildCfg(id, ownerID);
            }

            return guildCfg;
        }

        private static GuildCfg CreateGuildCfg(ulong id, ulong ownerID)
        {
            GuildCfg guildCfg = new GuildCfg()
            {
                GuildID = id,
                GuildOwnerID = ownerID
            };

            guildConfigs.Add(guildCfg);
            Save();
            return guildCfg;
        }
    }
}
