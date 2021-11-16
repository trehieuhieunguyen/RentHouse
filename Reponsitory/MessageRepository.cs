using Microsoft.EntityFrameworkCore;
using RentHouse.Data;
using RentHouse.Models;
using RentHouse.Models.Groups;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Reponsitory
{
    public class MessageRepository : IMessageRepository
    {
        private readonly ApplicationDbContext _db;

        public MessageRepository(ApplicationDbContext db)
        {
            _db = db;
        }

        public void AddGroup(Group group)
        {
            _db.Groups.Add(group);
        }

        public void AddMessage(MessageRoom message)
        {
            _db.messageRooms.Add(message);
        }

        public async Task<Connection> GetConnection(string connectionId)
        {
            return await _db.Connections.FindAsync(connectionId);
        }

        public async Task<Group> GetGroupForConnection(string connectionId)
        {
            return await _db.Groups
                .Include(c => c.Connections)
                .Where(c => c.Connections.Any(x => x.ConnectionId == connectionId))
                .FirstOrDefaultAsync();
        }


        public async Task<Group> GetMessageGroup(string groupName)
        {
            return await _db.Groups
                .Include(x => x.Connections)
                .FirstOrDefaultAsync(x => x.Name == groupName);
        }

        public async Task<IEnumerable<MessageRoom>> GetMessageThread(string currentUsername, int recipientProductId)
        {
            var messages = await _db.messageRooms
                .Include(x=>x.ApplicationUser)
                .Where(m => m.RoomHouse.Id == recipientProductId)
                .OrderBy(m => m.UpdateTime)
                .ToListAsync();


            return messages;
        }

        public void RemoveConnection(Connection connection)
        {
            _db.Connections.Remove(connection);
        }

        public bool HasChanges()
        {
            return _db.ChangeTracker.HasChanges();
        }

        public async Task<bool> SaveAllAsync()
        {
            return await _db.SaveChangesAsync() > 0;
        }

        public void UpdateMessage(MessageRoom message)
        {
            _db.messageRooms.Update(message);
        }

        public bool MessageisExits(string SenderId, int RecipientId)
        {
            return _db.messageRooms.Any(u => u.ApplicationUserId == SenderId && u.RoomHouseId == RecipientId);
        }

        public async Task<MessageRoom> GetMessagePrice(int recipientProductId)
        {
            return await _db.messageRooms
                .OrderByDescending(c => c.Text)
                .FirstOrDefaultAsync(m => m.RoomHouseId == recipientProductId);
        }

        public async Task<MessageRoom> GetBacktoMoney(int recipientProductId)
        {
            return await _db.messageRooms.FirstOrDefaultAsync(m => m.RoomHouseId == recipientProductId);
        }

        public async Task<MessageRoom> GetMessage(int RecipientId)
        {
            return await _db.messageRooms
                .Include(u => u.ApplicationUser)
                .Include(u => u.RoomHouse)
                .FirstOrDefaultAsync(x => x.RoomHouseId == RecipientId);
        }

        public bool CheckTop1(string SenderId, int RecipientId)
        {
            return _db.messageRooms.Any(u => u.ApplicationUserId == SenderId && u.RoomHouseId == RecipientId );
        }

        public async Task<MessageRoom> GetMessageDefault(string SenderId, int recipientProductId)
        {
            return await _db.messageRooms.FirstOrDefaultAsync(m => m.ApplicationUserId == SenderId && m.RoomHouseId == recipientProductId);
        }

      
    }
}
