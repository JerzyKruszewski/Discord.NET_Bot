using System;
using System.Collections.Generic;

namespace DiscordBOT.Core.Objects
{
    public class UserNicknames
    {
        public ulong ID { get; set; }

        public List<string> Nicknames { get; set; }

        public DateTime TimeToNickChange { get; set; } = DateTime.Now;
    }
}
