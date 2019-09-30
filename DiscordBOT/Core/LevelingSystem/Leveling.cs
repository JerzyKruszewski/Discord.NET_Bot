using Discord.WebSocket;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core.LevelingSystem
{
    internal static class Leveling
    {
        internal static async void UserSendMessage(SocketUser user)
        {
            UserExpMute account = UsersExpMute.GetExpMute(user.Id);

            account.XP += 50;

            UsersExpMute.Save();
        }
    }
}
