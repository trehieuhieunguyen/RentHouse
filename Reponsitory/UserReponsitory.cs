using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using RentHouse.Data;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Reponsitory
{
    public class UserReponsitory : IUserReponsitory
    {
        private readonly UserManager<IdentityUser> _userManager;
        private readonly ApplicationDbContext _db;
       
        public UserReponsitory( ApplicationDbContext db,UserManager<IdentityUser> userManager)
        {
          _db = db;
          _userManager = userManager;
        }
        public async Task<ApplicationUser> GetUserByIdAsync(string id)
        {
            return await _db.applicationUsers.FirstOrDefaultAsync(find => find.Id == id);
        }
        public bool UpdateUser(ApplicationUser applicationUser)
        {
            _db.applicationUsers.Update(applicationUser);
            return SaveChange();
        }
        public async Task<bool>  DeleteUser(string id)
        {
            var x = await GetUserByIdAsync(id);
            if (x.LockoutEnabled == true)
            {
                x.LockoutEnabled = false;
            }else
            if (x.LockoutEnabled == false)
            {
                x.LockoutEnabled = true;
            }
            _db.applicationUsers.Update(x);
            return SaveChange();
        }
        public async Task<bool> CheckUserName(string username)
        {
            var users = await _db.applicationUsers.ToListAsync();
            foreach(var user in users)
            {
                if (user.UserName == username)
                {
                    return true;
                }
            }
            return false;
        }
        public async Task<IOrderedQueryable<ApplicationUser>> GetUserAdmin(string nameinput)
        {
            if (string.IsNullOrWhiteSpace(nameinput)) nameinput = "";
            var users =await _userManager.Users.ToListAsync();
            foreach(var user in users)
            {
               var role = await _userManager.GetRolesAsync(user);
                if (role.Contains("Admins"))
                {
                    var username = _db.applicationUsers.AsNoTracking().Where(y => y.UserName.Contains(nameinput)&&y.Id!=user.Id).OrderBy(x => x.UserName);
                    return username;
                }
            }         
            return null;
        }
        public async Task<IOrderedQueryable<House>> GetHouse(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) title = "";

            var house = _db.houses.AsNoTracking().Where(x => x.NameHourse.Contains(title) || x.Id.ToString() == title).OrderBy(x => x.NameHourse);
            return house;
        }
        public bool SaveChange()
        {
            return _db.SaveChanges() > 0;
        }
        public bool CreateUser(ApplicationUser applicationUser)
        {
            _db.applicationUsers.Add(applicationUser);
            return SaveChange();
        }
    }
}
