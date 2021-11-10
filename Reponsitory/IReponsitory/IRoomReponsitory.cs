﻿using RentHouse.Models;

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
    }
}
