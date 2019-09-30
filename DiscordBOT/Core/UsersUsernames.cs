using System.Collections.Generic;
using System.Linq;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersUsernames
    {
        private static readonly List<UserUsernames> _usernames;

        private static readonly string _filePath = "Resources/Usernames";

        static UsersUsernames()
        {
            if (JsonStorage.FileExist(_filePath))
            {
                _usernames = JsonStorage.RestoreObject<List<UserUsernames>>(_filePath);
            }
            else
            {
                _usernames = new List<UserUsernames>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_usernames, _filePath);
        }

        public static UserUsernames GetUsernames(ulong id)
        {
            return GetUserUsernames(id);
        }

        private static UserUsernames GetUserUsernames(ulong id)
        {
            IEnumerable<UserUsernames> result = from u in _usernames
                                                where u.ID == id
                                                select u;

            UserUsernames names = result.FirstOrDefault();

            if (names == null)
            {
                names = CreateUserUsernames(id);
            }

            return names;
        }

        private static UserUsernames CreateUserUsernames(ulong id)
        {
            UserUsernames names = new UserUsernames()
            {
                ID = id,
                Usernames = new List<string>()
            };

            _usernames.Add(names);
            Save();

            return names;
        }

        public static List<UserUsernames> GetAllUsernames()
        {
            return _usernames;
        }
    }
}
