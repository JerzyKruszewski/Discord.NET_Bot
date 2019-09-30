using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersExpMute
    {
        private static readonly List<UserExpMute> _usersExp;

        private static readonly string _filepath = "Resources/Exp";

        static UsersExpMute()
        {
            if (JsonStorage.FileExist(_filepath))
            {
                _usersExp = JsonStorage.RestoreObject<List<UserExpMute>>(_filepath);
            }
            else
            {
                _usersExp = new List<UserExpMute>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_usersExp, _filepath);
        }

        public static UserExpMute GetExpMute(ulong id)
        {
            return GetUserExpMute(id);
        }

        private static UserExpMute GetUserExpMute(ulong id)
        {
            IEnumerable<UserExpMute> result = from u in _usersExp
                                                where u.ID == id
                                                select u;

            UserExpMute nicks = result.FirstOrDefault();

            if (nicks == null)
            {
                nicks = CreateUserExpMute(id);
            }

            return nicks;
        }

        private static UserExpMute CreateUserExpMute(ulong id)
        {
            UserExpMute nicks = new UserExpMute()
            {
                ID = id
            };

            _usersExp.Add(nicks);
            Save();

            return nicks;
        }
    }
}
