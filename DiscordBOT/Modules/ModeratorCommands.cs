using System;
using System.Linq;
using System.Collections.Generic;
using System.Threading.Tasks;
using Discord;
using Discord.Commands;
using Discord.WebSocket;
using Discord.Addons.Interactive;
using DiscordBOT.Configs;
using DiscordBOT.Preconditions;
using DiscordBOT.Core;
using DiscordBOT.Core.Objects;
using DiscordBOT.Miscellaneous;

namespace DiscordBOT.Modules
{
    public class ModeratorCommands : InteractiveBase<SocketCommandContext>
    {
        [Command("GiveRoleAsync")]
        [RequireAdminPermissionOrModeratorRole]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireHigherPosition]
        public async Task GR(IRole role)
        {
            await (Context.User as SocketGuildUser).AddRoleAsync(role);
        }

        [Command("RemoveRoleAsync")]
        [RequireAdminPermissionOrModeratorRole]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [RequireHigherPosition]
        public async Task RR(IRole role)
        {
            await (Context.User as SocketGuildUser).RemoveRoleAsync(role);
        }

        [Command("ban")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.BanMembers)]
        public async Task Ban(SocketGuildUser user, [Remainder]string reason)
        {
            await user.BanAsync(0, reason, null);
        }

        [Command("pochwal")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Pochwal gracza: user, z powodu: reason (WYMAGANE)")]
        public async Task Praise(SocketUser user, [Remainder]string reason)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);
            ISocketMessageChannel generalChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(Context.Guild, guildCfg.GeneralChannelID);
            UserPraises account = UsersPraises.GetUserPraises(user.Id);

            uint id = (uint)account.Praises.Count;

            Praise praise = Praises.CreatePraise(user, id, reason);

            account.Praises.Add(praise);
            UsersPraises.Save();

            await generalChannel.SendMessageAsync($"Użytkownik {user.Mention} otrzymał pochwałę od {Context.User.Username}. Powód: {reason}");
            await Context.Channel.SendMessageAsync("Dodano pochwałę.");
        }

        [Command("Upomnienie")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Upomnij gracza: user, z powodu: reason")]
        public async Task Reminder(SocketGuildUser user, [Remainder]string reason = "Naruszenie regulaminu serwera")
        {
            try
            {
                GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);
                IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();

                await dmChannel.SendMessageAsync($"{user.Mention} otrzymałeś upomnienie, powód: {reason}\nPolecam przeczytać regulamin dostępny na <#{guildCfg.StatuteChannelID}>.");
                await Context.Channel.SendMessageAsync($"Upom100: Dokładna treść wysłanej wiadomości:\n{user.Mention} otrzymałeś upomnienie, powód: {reason}\nPolecam przeczytać regulamin dostępny na <#{guildCfg.StatuteChannelID}>.");

            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        #region Warning
        [Command("Ostrzeżenie", RunMode = RunMode.Async)]
        [Alias("Warn", "Ostrzezenie")]
        [RequireAdminPermissionOrModeratorRole]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Summary("Ostrzeż gracza: user, z powodu: reason. W przeciwieństwie do upomnienia, ma wymierne konsekwencje.")]
        public async Task Warn(SocketGuildUser user, [Remainder]string reason = "Naruszenie regulaminu serwera")
        {
            Random random = new Random(DateTime.Now.Millisecond);
            uint id = (uint)random.Next(0, 100000);

            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            ISocketMessageChannel modChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(Context.Guild, guildCfg.ModeratorChannelID);

            UserWarnings account = UsersWarnings.GetUserWarnings(user);
            SocketRole role = Methods.GetRoleByID(Context.Guild, guildCfg.PunishmentRoleID);

            Methods.DeleteExpiredWarnings(account);

            Warning warning = Warnings.CreateWarning(id, reason, guildCfg.WarnDuration);
            account.Warnings.Add(warning);
            UsersWarnings.Save();

            string msg = "";
            string message = "";

            if (account.Warnings.Count >= 5)
            {
                await user.BanAsync(0, "Przekroczenie limitu ostrzeżeń.");
            }
            else if (account.Warnings.Count > 2)
            {
                await user.AddRoleAsync(role);
                msg = $"{user.Mention} przekroczyłeś limit dopuszczalnych ostrzeżeń, co spowodowało Twój pobyt w {role.Name}.\nPowód ostatniego ostrzeżenia: {reason}\nPolecam przeczytać regulamin dostępny na <#{guildCfg.StatuteChannelID}>.";
                message = $"KarcNaN: Przekroczono limit ostrzeżeń, co skutkuje pobytem {user.Mention} w Karcerze.\nDokładna treść wysłanej wiadomości:\n{msg}";
            }
            else
            {
                msg = $"{user.Mention} otrzymałeś ostrzeżenie, powód: {reason}\nMasz {account.Warnings.Count} ostrzeżeń (maksymalnie możesz mieć 2, każdy kolejny, to {role.Name}).\nPolecam przeczytać regulamin dostępny na <#{guildCfg.StatuteChannelID}>.";
                message = $"Ostrz101: Dokładna treść wysłanej wiadomości:\n{msg}";
            }

            await dmChannel.SendMessageAsync(msg);
            await modChannel.SendMessageAsync(message);
        }

        [Command("Usuń ostrzeżenie")]
        [Alias("Remove Warn", "Usun ostrzezenie")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Usuń ostrzeżenie z gracza: user, o numerze identydikacyjnym: id. Numer identyfikacyjny sprawdzamy przy pomocy komendy: $ostrzeżenia user")]
        public async Task RemoveWarning(SocketGuildUser user, uint id)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            ISocketMessageChannel modChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(Context.Guild, guildCfg.ModeratorChannelID);

            UserWarnings account = UsersWarnings.GetUserWarnings(user);
            Warning warning = Warnings.GetWarning(account, id);

            if (warning != null)
            {
                account.Warnings.Remove(warning);

                UsersWarnings.Save();

                await modChannel.SendMessageAsync($"Pomyślnie usunięto ostrzeżenie {warning.ID} z konta {user.Mention}.");
            }
            else
            {
                await modChannel.SendMessageAsync($"Coś poszło nie tak... Czy na pewno użyłeś dobrego ID? 🤔");
            }
        }
        #endregion

        [Command("GiveKarcerAsync")]
        [Alias("GiveKarcer", "Karcer")]
        [RequireAdminPermissionOrModeratorRole]
        [RequireBotPermission(GuildPermission.ManageRoles)]
        [Summary("Daj Karcer użytkownikowi: user na: time z powodu: reason")]
        public async Task GiveKarcerAsync(SocketGuildUser user, byte time, [Remainder]string reason)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            IDMChannel dmChannel = await user.GetOrCreateDMChannelAsync();
            ISocketMessageChannel modChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(Context.Guild, guildCfg.ModeratorChannelID);

            SocketRole role = Methods.GetRoleByID(Context.Guild, guildCfg.PunishmentRoleID);
            await user.AddRoleAsync(role);

            await dmChannel.SendMessageAsync($"Zostałeś wysłany na {time}h do {role.Name}, z powodu: {reason}");
            await modChannel.SendMessageAsync($"Karc{time}: Użytkownik {user.Mention} został wysłany na {time}h do {role.Name}, z powodu: {reason}");
        }

        [Command("Mute user")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [Summary("Ucisz/odcisz użytkownika: user. True - ucisz. False - odcisz.")]
        public async Task Mute(SocketGuildUser user, bool mute)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

            ISocketMessageChannel modChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(Context.Guild, guildCfg.ModeratorChannelID);

            UserExpMute account = UsersExpMute.GetExpMute(user.Id);

            account.IsMuted = mute;

            UsersExpMute.Save();

            await modChannel.SendMessageAsync($"Pomyślnie zmieniono możliwość swobodnej wypowiedzi użytkownikowi {user.Mention} na: {!account.IsMuted}.");
        }

        [Command("Give user xp")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Dodaj użytkownikowi: user, amount XP")]
        public async Task GiveXP(SocketGuildUser user, ulong amount)
        {
            UserExpMute account = UsersExpMute.GetExpMute(user.Id);

            account.XP += amount;

            UsersExpMute.Save();

            await Context.Channel.SendMessageAsync($"Dodano użytkownikowi {user.Mention} {amount} XP");
        }

        [Command("Message")]
        [Alias("msg")]
        [RequireUserPermission(GuildPermission.Administrator)]
        [Summary("Wysyła na kanał: channel wiadomość o treści msg")]
        public async Task Message(ISocketMessageChannel channel, [Remainder]string msg)
        {
            await channel.SendMessageAsync(msg);
        }

        [Command("Osiągnięcie")]
        [Alias("Osiagniecie")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Nadaj użytkownikowi: user, osiągnięcie: archievement")]
        public async Task AddUserArchievement(SocketUser user, [Remainder]string archievement)
        {
            UserArchievements account = UsersArchievements.GetUserArchievements(user.Id);

            account.Archievements.Add(archievement);

            UsersArchievements.Save();

            await Context.Channel.SendMessageAsync($"Dodano osiągnięcie: {archievement} Użytkownikowi {user.Mention}");
        }

        [Command("Allow nick change")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Pozwól użytkownikowi: user, na zmianę nicku, pomimo timeouta. NIE ZMIENIAĆ OSOBIŚCIE!!!")]
        public async Task AllowNickChange(SocketGuildUser user)
        {
            UserNicknames account = UsersNicknames.GetNicknames(user.Id);

            account.TimeToNickChange = DateTime.Now;

            UsersNicknames.Save();

            await Context.Channel.SendMessageAsync($"Zezwolono na zmianę nicku użytkownikowi {user.Mention}");
        }

        [Command("IdUser")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Znajdź nicki użytkownika.")]
        public async Task IdUser(SocketGuildUser user)
        {
            string result = "";
            UserNicknames n = UsersNicknames.GetNicknames(user.Id);
            UserUsernames u = UsersUsernames.GetUsernames(user.Id);

            foreach (string username in u.Usernames)
            {
                result += $"- {username}\n";

                if (result.Length > 1970)
                {
                    break;
                }
            }

            await Context.Channel.SendMessageAsync(result);

            result = "";

            foreach (string nickname in n.Nicknames)
            {
                result += $"- {nickname}\n";

                if (result.Length > 1970)
                {
                    break;
                }
            }

            await Context.Channel.SendMessageAsync(result);
        }

        [Command("Find User")]
        [RequireAdminPermissionOrModeratorRole]
        [Summary("Znajdź użytkownika, po kawałku nicku lub username: name (wielkość liter bez znaczenia)")]
        public async Task FindUser([Remainder]string name)
        {
            string result = "";
            List<ulong> accounts = FindUserLogic(name);

            if (accounts.Count != 0)
            {
                foreach (var id in accounts)
                {
                    result += $"- <@{id}> {id}\n";
                }

                await Context.Channel.SendMessageAsync(result);
            }
            else
            {
                await Context.Channel.SendMessageAsync("Nie znaleziono takiego użytkownika");
            }
        }

        private List<ulong> FindUserLogic(string name)
        {
            List<ulong> accounts = new List<ulong>();
            List<UserNicknames> n = UsersNicknames.GetAllNicknames();
            List<UserUsernames> u = UsersUsernames.GetAllUsernames();

            foreach (var user in u)
            {
                foreach (var username in user.Usernames)
                {
                    if (username.ToLower().Contains(name.ToLower()))
                    {
                        accounts.Add(user.ID);
                        break;
                    }
                }
            }

            foreach (var user in n)
            {
                foreach (var nickname in user.Nicknames)
                {
                    if (nickname.ToLower().Contains(name.ToLower()))
                    {
                        accounts.Add(user.ID);
                        break;
                    }
                }
            }

            return accounts;
        }

        [Command("czyść", RunMode = RunMode.Async)]
        [Alias("wyczyść", "clear")]
        [RequireAdminPermissionOrModeratorRole]
        [RequireBotPermission(GuildPermission.ManageMessages)]
        [Summary("usuń ilosc wiadomości na kanale channel.")]
        public async Task Clear(byte ilosc, ISocketMessageChannel channel = null)
        {
            if (channel == null)
            {
                channel = Context.Channel;
            }

            IEnumerable<IMessage> messages = await channel.GetMessagesAsync(ilosc).FlattenAsync();

            foreach (IMessage msg in messages)
            {
                await msg.DeleteAsync();
                await Task.Delay(200);
            }
        }
    }
}
