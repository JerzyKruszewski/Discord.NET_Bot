using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersWarnings
    {
        private static readonly List<UserWarnings> _usersWarnings;

        private static readonly string _filePath = "Resources/Warns";

        static UsersWarnings()
        {
            if (JsonStorage.FileExist(_filePath))
            {
                _usersWarnings = JsonStorage.RestoreObject<List<UserWarnings>>(_filePath);
            }
            else
            {
                _usersWarnings = new List<UserWarnings>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_usersWarnings, _filePath);
        }

        public static UserWarnings GetUserWarnings(SocketUser user)
        {
            return GetOrCreateUserWarnings(user);
        }

        private static UserWarnings GetOrCreateUserWarnings(SocketUser user)
        {
            var result = from u in _usersWarnings
                         where u.ID == user.Id
                         select u;

            UserWarnings userWarnings = result.FirstOrDefault();

            if (userWarnings == null)
            {
                return CreateUserWarnings(user);
            }

            return userWarnings;
        }

        private static UserWarnings CreateUserWarnings(SocketUser user)
        {
            UserWarnings uw = new UserWarnings()
            {
                ID = user.Id,
                Warnings = new List<Warning>()
            };

            _usersWarnings.Add(uw);
            Save();

            return uw;
        }
    }
}
