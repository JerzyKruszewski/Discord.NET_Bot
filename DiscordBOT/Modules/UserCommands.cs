using System;
using System.Threading.Tasks;
using System.Linq;
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
    public class UserCommands : InteractiveBase<SocketCommandContext>
    {
        [Command("change nick")]
        [RequireHigherPosition]
        [RequireBotPermission(GuildPermission.ChangeNickname)]
        [Summary("Zmienia nick wywołującemu na: nickname i zapisuje go do bazy.")]
        public async Task ChangeNickname([Remainder]string nickname)
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            UserNicknames account = UsersNicknames.GetNicknames(Context.User.Id);

            if (DateTime.Compare(DateTime.Now, account.TimeToNickChange) >= 0)
            {
                if (nickname.Length > 32)
                {
                    await Context.Channel.SendMessageAsync("Nick jest za długi. Przekroczyłeś odgórne limity Discorda.");
                }
                else
                {
                    await user.ModifyAsync(x =>
                    {
                        x.Nickname = nickname;
                    });

                    GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(Context.Guild);

                    account.Nicknames.Add(nickname);
                    account.TimeToNickChange = DateTime.Now + TimeSpan.FromDays(guildCfg.DaysTillNextNicknameChange);

                    UsersNicknames.Save();

                    await Context.Channel.SendMessageAsync($"Pomyślnie zmieniono pseudonim. Następna zmiana będzie możliwa dopiero {account.TimeToNickChange}");
                }
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Nie możesz zmienić pseudonimu. Będziesz to mógł zrobić dopiero {account.TimeToNickChange}");
            }
        }

        [Command("Check nick")]
        [Summary("sprawdza kiedy użytkownik może zmienić nick")]
        public async Task ChkNick()
        {
            SocketGuildUser user = (SocketGuildUser)Context.User;
            UserNicknames account = UsersNicknames.GetNicknames(Context.User.Id);

            if (DateTime.Compare(DateTime.Now, account.TimeToNickChange) >= 0)
            {
                await Context.Channel.SendMessageAsync("Możesz zmienić pseudonim.");
            }
            else
            {
                await Context.Channel.SendMessageAsync($"Nie możesz zmienić pseudonimu. Będziesz to mógł zrobić dopiero {account.TimeToNickChange}");
            }
        }

        [Command("status", RunMode = RunMode.Async)]
        [Summary("Sprawdza status użytkownika: user")]
        public async Task CheckStatus(SocketUser user = null)
        {
            if (user == null)
            {
                user = Context.User;
            }

            UserExpMute userExp = UsersExpMute.GetExpMute(user.Id);
            UserPraises uPraises = UsersPraises.GetUserPraises(user.Id);
            UserArchievements uArchievements = UsersArchievements.GetUserArchievements(user.Id);
            UserWarnings accountWarns = UsersWarnings.GetUserWarnings(user);

            Methods.DeleteExpiredWarnings(accountWarns);

            string archievements = "Osiągnięcia:\n";

            foreach (string item in uArchievements.Archievements)
            {
                archievements += $"{item}\n";
            }

            await Context.Channel.SendMessageAsync($"{user.Username} ma {userExp.LevelNumber} lvl, {userExp.XP} xp. Został pochwalony {uPraises.Praises.Count} razy oraz otrzymał {accountWarns.Warnings.Count} ostrzeżeń.\n{archievements}");
        }

        [Command("pochwały")]
        [Alias("pochwaly")]
        [Summary("Sprawdza pochwały użytkownika: user")]
        public async Task CheckPraises(SocketUser user = null)
        {
            if (user == null)
            {
                user = Context.User;
            }

            UserPraises account = UsersPraises.GetUserPraises(user.Id);

            string praises = $"Pochwały {user.Username}:\n";

            foreach (Praise praise in account.Praises)
            {
                praises += $"{praise.ID} - {praise.Reason} ({praise.GivenAt})\n";
            }

            await Context.Channel.SendMessageAsync(praises);
        }

        [Command("ostrzeżenia", RunMode = RunMode.Async)]
        [Alias("ostrzezenia")]
        [Summary("Sprawdza ostrzeżenia użytkownika: user")]
        public async Task CheckWarnings(SocketUser user = null)
        {
            if (user == null)
            {
                user = Context.User;
            }

            UserWarnings account = UsersWarnings.GetUserWarnings(user);

            Methods.DeleteExpiredWarnings(account);

            string warns = $"Ostrzeżenia {user.Username}:\n";

            foreach (Warning warn in account.Warnings)
            {
                warns += $"{warn.ID} - {warn.Reason} ({warn.ExpireDate})\n";
            }

            await Context.Channel.SendMessageAsync(warns);
        }

        [Command("głosowanie")]
        [Alias("glosowanie")]
        [Summary("Konfiguruje wiadomość użytkownika pod głosowanie")]
        public async Task Vote([Remainder]string msg = null)
        {
            Emoji yes = new Emoji("❤");
            Emoji wait = new Emoji("✋");
            Emoji no = new Emoji("👎");

            IEmote[] emotes = new IEmote[3] { yes, wait, no };

            await Context.Message.AddReactionsAsync(emotes);
        }
    }
}
