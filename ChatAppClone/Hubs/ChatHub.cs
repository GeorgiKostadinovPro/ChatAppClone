﻿namespace ChatAppClone.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    public class ChatHub : Hub 
    {
        public async Task JoinChat(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChat(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
