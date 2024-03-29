﻿using System;
using System.Threading.Tasks;
using Discord.Commands;
using Discord.WebSocket;

namespace DiscordBOT.Preconditions
{
    public class RequireHigherPosition : PreconditionAttribute
    {
        public override Task<PreconditionResult> CheckPermissionsAsync(ICommandContext context, CommandInfo command, IServiceProvider services)
        {
            SocketGuildUser user = (SocketGuildUser)context.User;
            SocketGuildUser bot = (SocketGuildUser)context.Guild.GetUserAsync(context.Client.CurrentUser.Id).GetAwaiter().GetResult();

            if (bot.Hierarchy > user.Hierarchy)
            {
                return Task.FromResult(PreconditionResult.FromSuccess());
            }
            else
            {
                return Task.FromResult(PreconditionResult.FromError($"Nie masz uprawnień"));
            }
        }
    }
}
