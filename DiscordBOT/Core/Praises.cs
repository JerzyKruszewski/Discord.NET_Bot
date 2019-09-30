using System;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class Praises
    {
        public static Praise GetPraise(UserPraises user, uint id)
        {
            var result = from p in user.Praises
                         where p.ID == id
                         select p;

            Praise praise = result.FirstOrDefault();

            return praise;
        }

        public static Praise CreatePraise(SocketUser user, uint id, string reason)
        {
            Praise praise = new Praise
            {
                ID = id,
                GiverID = user.Id,
                Reason = reason,
                GivenAt = DateTime.Now
            };

            return praise;
        }
    }
}
