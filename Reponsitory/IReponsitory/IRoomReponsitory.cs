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
        Task<IOrderedQueryable<RoomHouse>> GetRoomHouseForUser(string title,string pricerent);
        Task<RoomHouse> GetRoomHouseForUserbyId(int id);
        Task<ICollection<MessageRoom>> GetCommentRooms(int id);
        bool SaveComment(MessageRoom comment);
        Task<RoomHouse> GetRoomForAdmin(int id);
        bool CreateImageRoom(IList<ImageUploadOfRoom> imageUploadOfRoom);
        Task<ICollection<ImageUploadOfRoom>> imageUploadOfRooms(int id);
        bool RemoveImgae(ICollection<ImageUploadOfRoom> imageUploadOfRoom);
        bool EditRoom(RoomHouse roomhouse);
        bool CheckRoomNumberCreate(int roomnumber, int houseId);
        bool CheckRoomNumberEdit(int roomnumber, int houseId, int roomId);
        bool CreateStar(RatingStar rating);
        bool CheckExistsStarOfUser(string id, int roomid);
        int StarOfUser(string id, int roomid);
        Task<double> StarOfRoom(int roomid);
        Task<ICollection<HistoryPay>> GetHistory(string Id);
        bool CreatePayPal(HistoryPay historyPay);
        Task<string> GetHouseOfUser(int houseId);
        bool CheckRoomNumberChange(int roomnumber, int houseId);
        bool CheckRoomNumberHadRent(int roomnumber, int houseId);
        Task<HistoryPay> GetHistoryForAdmin(int roomId);
        bool EditHistory(HistoryPay historyPay);
         Task<RoomHouse> GetRoomEdit(int houseId, int roomnumber);
        bool SaveMutilRoom(IList<RoomHouse> roomHouses);
    }
}
