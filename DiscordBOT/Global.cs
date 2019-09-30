using Discord.WebSocket;

namespace DiscordBOT
{
    internal static class Global
    {
        internal static DiscordSocketClient Client { get; set; }

        internal static ulong MessageIDToTrack { get; set; }
    }
}

