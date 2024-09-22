﻿namespace ChatAppClone.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub 
    {
        public async Task JoinChatGroup(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChatGroup(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
