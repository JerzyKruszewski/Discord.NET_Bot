using System;

namespace DiscordBOT.Core.Objects
{
    public class Warning
    {
        public uint ID { get; set; }

        public string Reason { get; set; }

        public DateTime GivenAt { get; set; } = DateTime.Now;

        public DateTime ExpireDate { get; set; }
    }
}
