using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBOT.Core.Objects
{
    public class UserArchievements
    {
        public ulong ID { get; set; }

        public List<string> Archievements { get; set; } = new List<string>();
    }
}
