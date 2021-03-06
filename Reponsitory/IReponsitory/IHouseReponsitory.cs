using RentHouse.Models;

namespace RentHouse.Reponsitory.IReponsitory
{
    public interface IHouseReponsitory
    {
        Task<IOrderedQueryable<House>> GetHouse(string title);
        Task<House> GetHouseById(int id);
        Task<bool> CreateHouse(House house);
        Task<bool> CreateImgae(IList<ImageUpload> imageUpload);
        Task<bool> SaveChange();
        Task<bool> Delete(int id);
        Task<bool> EditHouse(House house);
        Task<ICollection<ImageUpload>> GetSubImage(int id);
        Task<bool> CheckNameHouse(string Name, int Id);
        Task<bool> RemoveImgae(ICollection<ImageUpload> imageUpload);
        Task<bool> CreateHouseOfUser(HouseOfUser houseOfUser);
        Task<ICollection<RoomHouse>> getRoomInHouse(int id);
        Task<bool> CheckNameCreateHouse(string Name);
        Task<ICollection<House>> GetHouseForUser(string id);
        Task<ICollection<House>> getConfirmHouse();
    }
}
