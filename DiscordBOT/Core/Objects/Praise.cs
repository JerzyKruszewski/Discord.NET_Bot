using System;

namespace DiscordBOT.Core.Objects
{
    public class Praise
    {
        public uint ID { get; set; }

        public ulong GiverID { get; set; }

        public string Reason { get; set; }

        public DateTime GivenAt { get; set; } = DateTime.Now;
    }
}
