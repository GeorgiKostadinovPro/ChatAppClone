namespace ChatAppClone.Hubs
{
    using Microsoft.AspNetCore.SignalR;

    /// <summary>
    /// high-level pipeline for handling the real-time chat
    /// OnConnectedAsync() is invoked automatically to handle the initial connection
    /// OnDisconnectedAsync() is invoked automatically to close the initial connection
    /// </summary>
    public class ChatHub : Hub
    {
        public async Task JoinChatAsync(string chatId)
        {
            await Groups.AddToGroupAsync(Context.ConnectionId, chatId);
        }

        public async Task LeaveChatAsync(string chatId)
        {
            await Groups.RemoveFromGroupAsync(Context.ConnectionId, chatId);
        }
    }
}
