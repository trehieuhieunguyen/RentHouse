using RentHouse.Models;
using RentHouse.Models.Groups;

namespace RentHouse.Reponsitory.IReponsitory
{
    public interface IMessageRepository
    {
        void AddGroup(Group group);
        void RemoveConnection(Connection connection);
        Task<Connection> GetConnection(string connectionId);
        Task<Group> GetMessageGroup(string groupName);
        Task<Group> GetGroupForConnection(string connectionId);
        void AddMessage(MessageRoom message);
        void UpdateMessage(MessageRoom message);
        Task<MessageRoom> GetMessage(int RecipientId);
        Task<IEnumerable<MessageRoom>> GetMessageThread(string currentUsername, int recipientProductId);
        bool HasChanges();
        bool MessageisExits(string SenderId, int RecipientId);
        Task<bool> SaveAllAsync();
    }
}
