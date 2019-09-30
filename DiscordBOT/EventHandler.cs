using System;
using System.Linq;
using System.Threading.Tasks;
using System.Text;
using System.Net;
using System.Net.Mail;
using Discord.WebSocket;
using Discord.Rest;
using DiscordBOT.Configs;
using DiscordBOT.Miscellaneous;
using DiscordBOT.Core;
using DiscordBOT.Core.Objects;

namespace DiscordBOT
{
    public class EventHandler
    {
        public static async Task UserBanned(SocketUser arg1, SocketGuild arg2)
        {
            GuildCfg guildCfg = GuildsCfgs.GetGuildCfg(arg2);
            ISocketMessageChannel modChannel = (ISocketMessageChannel)Methods.GetTextChannelByID(arg2, guildCfg.ModeratorChannelID);

            RestBan ban = arg2.GetBansAsync().Result.ToList().FirstOrDefault(x => x.User.Id == arg1.Id);

            await modChannel.SendMessageAsync($"{ban.User.Mention} ({ban.User.Id}) otrzymał bana. Powód: {ban.Reason}");
        }

        public static async Task UserUpdated(SocketUser arg1, SocketUser arg2)
        {
            //arg1 - old
            //arg2 - new

            UserUsernames account;

            if (arg2.Username != arg1.Username)
            {
                account = UsersUsernames.GetUsernames(arg2.Id);
                account.Usernames.Add(arg2.Username);
                UsersUsernames.Save();
            }
        }

        public static Task BotConnected()
        {
            SendMail($"Connected {DateTime.Now}", $"Hej Jurij!\n\nPołączyłam się właśnie na serwery.");

            return Task.CompletedTask;
        }

        public static Task BotDisconnected(Exception arg)
        {
            string msg = $"Musiałam odłączyć się z serwera.\nPowód: {arg.Message}\nŹródło: {arg.Source}";

            SendMail($"Disconnected {DateTime.Now}", msg);

            return Task.CompletedTask;
        }

        private static void SendMail(string title, string msg)
        {
            int port = 587;
            string smtpHost = "smtp.gmail.com";
            string botEmail = "BotEmail@example.com";
            string botEmailPassword = "1234";
            string yourEmail = "you@example.com";

            SmtpClient client = new SmtpClient();
            client.Port = port;
            client.Host = smtpHost;
            client.EnableSsl = true;
            client.Timeout = 10000;
            client.DeliveryMethod = SmtpDeliveryMethod.Network;
            client.UseDefaultCredentials = false;
            client.Credentials = new NetworkCredential(botEmail, botEmailPassword);

            MailMessage mm = new MailMessage(botEmail, yourEmail, title, msg);
            mm.BodyEncoding = UTF8Encoding.UTF8;
            mm.DeliveryNotificationOptions = DeliveryNotificationOptions.OnFailure;

            client.Send(mm);

            client.Dispose();
            mm.Dispose();

            Console.WriteLine("Email Send");
        }
    }
}
