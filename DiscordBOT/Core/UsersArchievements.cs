using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersArchievements
    {
        private static readonly List<UserArchievements> _usersArchievements;

        private static readonly string _filePath = "Resources/Archievements";

        static UsersArchievements()
        {
            if (JsonStorage.FileExist(_filePath))
            {
                _usersArchievements = JsonStorage.RestoreObject<List<UserArchievements>>(_filePath);
            }
            else
            {
                _usersArchievements = new List<UserArchievements>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_usersArchievements, _filePath);
        }

        public static UserArchievements GetUserArchievements(ulong id)
        {
            return GetOrCreateUserArchievements(id);
        }

        private static UserArchievements GetOrCreateUserArchievements(ulong id)
        {
            var result = from u in _usersArchievements
                         where u.ID == id
                         select u;

            UserArchievements userArchievements = result.FirstOrDefault();

            if (userArchievements == null)
            {
                return CreateUserArchievements(id);
            }

            return userArchievements;
        }

        private static UserArchievements CreateUserArchievements(ulong id)
        {
            UserArchievements ua = new UserArchievements()
            {
                ID = id,
                Archievements = new List<string>()
            };

            _usersArchievements.Add(ua);
            Save();

            return ua;
        }
    }
}
