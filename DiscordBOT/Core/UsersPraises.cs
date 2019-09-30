using System;
using System.Collections.Generic;
using System.Text;
using System.Linq;
using Discord.WebSocket;
using DiscordBOT.Storage;
using DiscordBOT.Core.Objects;

namespace DiscordBOT.Core
{
    public class UsersPraises
    {
        private static readonly List<UserPraises> _usersPraises;

        private static readonly string _filePath = "Resources/Praises";

        static UsersPraises()
        {
            if (JsonStorage.FileExist(_filePath))
            {
                _usersPraises = JsonStorage.RestoreObject<List<UserPraises>>(_filePath);
            }
            else
            {
                _usersPraises = new List<UserPraises>();
                Save();
            }
        }

        public static void Save()
        {
            JsonStorage.StoreObject(_usersPraises, _filePath);
        }

        public static UserPraises GetUserPraises(ulong id)
        {
            return GetOrCreateUserPraises(id);
        }

        private static UserPraises GetOrCreateUserPraises(ulong id)
        {
            var result = from u in _usersPraises
                         where u.ID == id
                         select u;

            UserPraises userPraises = result.FirstOrDefault();

            if (userPraises == null)
            {
                return CreateUserPraises(id);
            }

            return userPraises;
        }

        private static UserPraises CreateUserPraises(ulong id)
        {
            UserPraises up = new UserPraises()
            {
                ID = id,
                Praises = new List<Praise>()
            };

            _usersPraises.Add(up);
            Save();

            return up;
        }
    }
}

