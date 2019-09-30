using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBOT.Core.Objects
{
    public class UserPraises
    {
        public ulong ID { get; set; }

        public List<Praise> Praises { get; set; }
    }
}
