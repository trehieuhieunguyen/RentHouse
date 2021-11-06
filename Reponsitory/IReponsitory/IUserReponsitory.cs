using RentHouse.Models;

namespace RentHouse.Reponsitory.IReponsitory
{
    public interface IUserReponsitory
    {
        Task<ApplicationUser> GetUserByIdAsync(string id);
        Task<IOrderedQueryable<ApplicationUser>> GetUserAdmin(string name);
        Task<bool> CheckUserName(string username);
        bool CreateUser(ApplicationUser applicationUser);
        bool SaveChange();
        bool UpdateUser(ApplicationUser applicationUser);
        Task<bool> DeleteUser(string id);
    }
}
