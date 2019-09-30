using System;
using System.Collections.Generic;
using System.Text;

namespace DiscordBOT.Core.Objects
{
    public class UserWarnings
    {
        public ulong ID { get; set; }

        public List<Warning> Warnings { get; set; }
    }
}
