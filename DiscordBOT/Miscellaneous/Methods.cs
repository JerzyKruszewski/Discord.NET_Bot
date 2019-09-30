using System;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Core;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Miscellaneous
{
    public class Methods
    {
        public static SocketGuildChannel GetTextChannelByID(SocketGuild guild, ulong channelID)
        {
            return guild.Channels.FirstOrDefault(x => x.Id == channelID);
        }

        public static SocketRole GetRoleByID(SocketGuild guild, ulong roleID)
        {
            return guild.Roles.FirstOrDefault(x => x.Id == roleID);
        }

        public static SocketUser GetUserByID(SocketGuild guild, ulong userID)
        {
            return guild.Users.FirstOrDefault(x => x.Id == userID);
        }

        public static void DeleteExpiredWarnings(UserWarnings account)
        {
            try
            {
                for (int i = account.Warnings.Count - 1; i >= 0; i--)
                {
                    if (DateTime.Compare(DateTime.Now, account.Warnings[i].ExpireDate) >= 0)
                    {
                        account.Warnings.RemoveAt(i);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }

            UsersWarnings.Save();
        }
    }
}
