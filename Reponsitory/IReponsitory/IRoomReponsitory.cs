using RentHouse.Models;

namespace RentHouse.Reponsitory.IReponsitory
{
    public interface IRoomReponsitory
    {
        Task<IOrderedQueryable<RoomHouse>> GetRoomHouse(string title);
        bool CreateRoom(RoomHouse roomHouse);
        bool SaveChange();
        bool CheckHouseId(int id);
        bool CheckNumberRoom(int houseId, int numberRoom);
         Task<bool> DeleteRoom(int id);
        Task<ICollection<RoomHouse>> GetRoomHouseForUser(string title,string pricerent);
        Task<RoomHouse> GetRoomHouseForUserbyId(int id);
    }
}
