using ChatSystem.Services.Services.Contracts;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.SignalR;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;

namespace ChatSystem.Web.Hubs
{
    [Authorize]
    public class ChatHub : Hub
    {
        private readonly IConversationService _conversationService;
        private readonly IChatService _chatService;
        private readonly IUserService _userService;
        private readonly ILogger _logger;
        private static readonly ConcurrentDictionary<string, List<string>> GroupConnections = new ConcurrentDictionary<string, List<string>>();

        public ChatHub(IConversationService conversationService, IChatService chatService, IUserService userService, ILogger<ChatHub> logger)
        {
            _conversationService = conversationService;
            _chatService = chatService;
            _userService = userService;
            _logger = logger;
        }

        public async Task SendMessage(int recipientId, string message)
        {
            if (recipientId == default(int) || string.IsNullOrEmpty(message))
            {
                // Handle invalid input
                await Clients.Caller.SendAsync("Invalid Input");
                return;
            }

            var senderUserId = _userService.GetCurrentUserId();
            var senderUsername = _userService.GetCurrentUserUsername();

            try
            {
                var recipientConnectionIds = GetRecipientConnectionIds(recipientId.ToString());

                if (recipientConnectionIds.Any())
                {
                    // Send the message to all connections of the recipient
                    foreach (var connectionId in recipientConnectionIds)
                    {
                        await Clients.Client(connectionId).SendAsync("ReceiveMessage", message, senderUsername);
                    }
                }

                var conversationId = await _conversationService.CreateConversationAsync(senderUserId, recipientId);
                await _chatService.AddChatMessageAsync(senderUserId, conversationId, message);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, ex.Message);
                await Clients.Caller.SendAsync("Send Message Failed");
            }
        }

        public override async Task OnConnectedAsync()
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string connectionId = Context.ConnectionId;

            // Add the connection to the user's group based on their userId
            await Groups.AddToGroupAsync(connectionId, userId);

            // Add the connection to the dictionary for tracking
            GroupConnections.AddOrUpdate(userId, new List<string> { connectionId }, (key, list) =>
            {
                list.Add(connectionId);
                return list;
            });

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            string userId = Context.User.FindFirstValue(ClaimTypes.NameIdentifier);
            string connectionId = Context.ConnectionId;

            // Remove the connection from the user's group
            await Groups.RemoveFromGroupAsync(connectionId, userId);

            // Remove the connection from the dictionary
            if (GroupConnections.TryGetValue(userId, out var connections))
            {
                connections.Remove(connectionId);

                // Remove the group from the dictionary if no connections are left
                if (connections.Count == 0)
                {
                    GroupConnections.TryRemove(userId, out _);
                }
            }

            await base.OnDisconnectedAsync(exception);
        }

        private IEnumerable<string> GetRecipientConnectionIds(string recipientId)
        {
            if (GroupConnections.TryGetValue(recipientId, out var connections))
            {
                return connections;
            }

            return Enumerable.Empty<string>();
        }
    }
}
