using System;
using System.Linq;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class Warnings
    {
        public static Warning GetWarning(UserWarnings user, uint id)
        {
            var result = from w in user.Warnings
                         where w.ID == id
                         select w;

            Warning warning = result.FirstOrDefault();

            return warning;
        }

        public static Warning CreateWarning(uint id, string reason, uint daysToExpire)
        {
            Warning warning = new Warning
            {
                ID = id,
                Reason = reason,
                ExpireDate = DateTime.Now + TimeSpan.FromDays(daysToExpire)
            };

            return warning;
        }
    }
}
