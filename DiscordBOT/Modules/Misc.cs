using System;
using System.Threading.Tasks;
using Discord.Commands;
using DiscordBOT.Preconditions;

namespace DiscordBOT.Modules
{
    public class Misc : ModuleBase<SocketCommandContext>
    {

        [Command("Test")]
        [RequireHigherPosition]
        public async Task test()
        {
            await Context.Channel.SendMessageAsync("Success!");
        }

        [Command("Debug")]
        [RequireOwner]
        public async Task debugmsg()
        {
            await Context.Channel.SendMessageAsync("Access Granded!");
        }

        [Command("RR")]
        public async Task RussianRoulette()
        {
            await User();

            Random random = new Random();
            int n = random.Next(1, 7);

            if (n==2)
            {
                await Lost();
            }
            else
            {
                await Win();
            }
        }

        [Command("Lost")]
        public async Task Lost()
        {
            await Context.Channel.SendMessageAsync(":ghost:");
            await Context.Channel.SendMessageAsync($":dizzy_face: :gun: {Context.User.Mention}");
        }

        [Command("Win")]
        public async Task Win()
        {
            await Context.Channel.SendMessageAsync($":joy: :gun: {Context.User.Mention}");
        }

        [Command("!kill")]
        public async Task Kill([Remainder]string username = "Twoja Stara Zapierdala")
        {
            Random random = new Random();
            int n = random.Next(1, 11);

            await Context.Channel.SendMessageAsync($"{username} --> :scream:             :gun: :joy: <-- {Context.User.Username}");
            if (n > 5)
            {
                await Context.Channel.SendMessageAsync(":boom: BOOM! :boom:");
                await Context.Channel.SendMessageAsync($"{username} --> :dizzy_face:             :gun: :joy: <-- {Context.User.Username}");
            }
            else if (n <= 5)
            {
                await Context.Channel.SendMessageAsync("Click!");
                await Context.Channel.SendMessageAsync($"{username} --> :joy: :knife:            :flushed: <-- {Context.User.Username}");
                await Context.Channel.SendMessageAsync($"{username} -->             :joy: :knife: :dizzy_face: <-- {Context.User.Username}");
            }
        }

        [Command("user")]
        public async Task User(Discord.WebSocket.SocketUser user = null)
        {
            if (user == null)
            {
                user = Context.User;
                Console.Write($"{user.Username}: ");
            }
        }

        [Command("K4")]
        [Alias("d4")]
        public async Task RollK4()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 5);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K6")]
        [Alias("d6")]
        public async Task RollK6()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 7);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K8")]
        [Alias("d8")]
        public async Task RollK8()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 9);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K10")]
        [Alias("d10")]
        public async Task RollK10()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 11);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K12")]
        [Alias("d12")]
        public async Task RollK12()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 13);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K20")]
        [Alias("d20")]
        public async Task RollK20()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 21);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K00-90")]
        [Alias("d00-90")]
        public async Task RollK0090()
        {
            await User();
            Random random = new Random();
            int n = (random.Next(0, 10))*10;
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }

        [Command("K100")]
        [Alias("d100")]
        public async Task RollK100()
        {
            await User();
            Random random = new Random();
            int n = random.Next(1, 101);
            await Context.Channel.SendMessageAsync(Convert.ToString(n));
        }
    }
}
