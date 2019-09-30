using System;

namespace DiscordBOT.Core.Objects
{
    public class UserExpMute
    {
        public ulong ID { get; set; }

        public ulong XP { get; set; } = 0;

        public uint LevelNumber
        {
            get
            {
                return (uint)Math.Sqrt(XP / 50);
            }
        }

        public bool IsMuted { get; set; } = false;
    }
}
