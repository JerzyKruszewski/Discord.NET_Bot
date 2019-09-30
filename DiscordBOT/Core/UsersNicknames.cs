using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersNicknames
    {
        private static readonly List<UserNicknames> _nicknames;

        private static readonly string _filePath = "Resources/Nicknames";

        static UsersNicknames()
        {
            if (JsonStorage.FileExist(_filePath))
            {
                _nicknames = JsonStorage.RestoreObject<List<UserNicknames>>(_filePath);
            }
            else
            {
                _nicknames = new List<UserNicknames>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_nicknames, _filePath);
        }

        public static UserNicknames GetNicknames(ulong id)
        {
            return GetUserNicknames(id);
        }

        private static UserNicknames GetUserNicknames(ulong id)
        {
            IEnumerable<UserNicknames> result = from n in _nicknames
                                                where n.ID == id
                                                select n;

            UserNicknames nicks = result.FirstOrDefault();

            if (nicks == null)
            {
                nicks = CreateUserNicknames(id);
            }

            return nicks;
        }

        private static UserNicknames CreateUserNicknames(ulong id)
        {
            UserNicknames nicks = new UserNicknames()
            {
                ID = id,
                Nicknames = new List<string>()
            };

            _nicknames.Add(nicks);
            Save();

            return nicks;
        }

        public static List<UserNicknames> GetAllNicknames()
        {
            return _nicknames;
        }
    }
}
