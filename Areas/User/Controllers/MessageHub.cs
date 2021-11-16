using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.SignalR;
using RentHouse.Extensions;
using RentHouse.Models;
using RentHouse.Models.Groups;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admins")]
    public class MessageHub : Hub
    {
        private readonly IMessageRepository _messageRepository;
        public MessageHub(IMessageRepository messageRepository)
        {
            _messageRepository = messageRepository;
        }

        public override async Task OnConnectedAsync()
        {
            var httpContext = Context.GetHttpContext();
            var OtherProductId = httpContext.Request.Query["RecipientProductId"].ToString();
            var groupName = GetGroupName(OtherProductId.ToString());
            await Groups.AddToGroupAsync(Context.ConnectionId, groupName);
            await AddToGroup(groupName);

            var messages = await _messageRepository.
                GetMessageThread(Context.User.GetUsername(), int.Parse(OtherProductId));

            await Clients.Caller.SendAsync("ReceiveMessageThread", messages);

        }

        public override async Task OnDisconnectedAsync(Exception exception)
        {
            var group = await RemoveFromMessageGroup();
            await base.OnDisconnectedAsync(exception);
        }

        public async Task SendMessageProduct(MessageRoom messageRoom)
        {
            string groupName = GetGroupName(messageRoom.Id.ToString());
            var message = new MessageRoom
            {
                ApplicationUserId = messageRoom.ApplicationUserId,
                Text = messageRoom.Text,
                RoomHouseId = messageRoom.RoomHouseId,
                UpdateTime = DateTime.Now
                
            };
            _messageRepository.AddMessage(message);
            await Clients.Group(groupName).SendAsync("NewMessage", messageRoom);  

        }

        private async Task<Group> AddToGroup(string groupName)
        {
            var group = await _messageRepository.GetMessageGroup(groupName);
            var connection = new Connection(Context.ConnectionId, Context.User.GetUsername());

            if (group == null)
            {
                group = new Group(groupName);
                _messageRepository.AddGroup(group);
            }

            group.Connections.Add(connection);

            if (await _messageRepository.SaveAllAsync()) return group;

            throw new HubException("Failed to join group");
        }

        private async Task<Group> RemoveFromMessageGroup()
        {
            var group = await _messageRepository.GetGroupForConnection(Context.ConnectionId);
            var connection = group.Connections.FirstOrDefault(x => x.ConnectionId == Context.ConnectionId);
            _messageRepository.RemoveConnection(connection);
            if (await _messageRepository.SaveAllAsync()) return group;

            throw new HubException("Failed to remove from group");
        }

        private string GetGroupName(string ProductNameId)
        {
            return $"{ProductNameId}";
        }
    }
}
