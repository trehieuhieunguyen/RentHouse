using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using ReflectionIT.Mvc.Paging;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = "Admins")]
    public class UserController : Controller
    {
        private readonly IUserReponsitory _res;
        private readonly UserManager<IdentityUser> _userManager;
        public UserController(IUserReponsitory res, UserManager<IdentityUser> userManager)
        {
            _res = res;
            _userManager = userManager;
        }

        public async Task<IActionResult>  Index(int pageIndex = 1, [FromQuery(Name = ("inputuser"))] string inputuser = "")
        {
            ViewBag.keyworduser= inputuser;
            var applicationUsers = await _res.GetUserAdmin(ViewBag.keyworduser);
            var result = await PagingList<ApplicationUser>.CreateAsync(applicationUsers, 2, pageIndex);
            return View(result);
        }
        
        public async Task<IActionResult> CreateUser()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateUser(ApplicationUser applicationUser)
        {
            applicationUser.EmailConfirmed = true;
            applicationUser.LockoutEnabled = true;
            applicationUser.UserName = applicationUser.Email;
            if (ModelState.IsValid)
            {
                    await _userManager.CreateAsync(applicationUser, applicationUser.PasswordHash);
                    await _userManager.AddToRoleAsync(applicationUser, "User");
                    return RedirectToAction("Index");
                
            }
            return View();
        }
        public async Task<IActionResult> EditUser(string id)
        {
            var user = await _res.GetUserByIdAsync(id);
            return View(user);
        }
        [HttpPost]
        public async Task<IActionResult> EditUser(ApplicationUser applicationUser,string roleofuser,string lockuser)
        {
            var user = await _res.GetUserByIdAsync(applicationUser.Id);
            user.FirtsName = applicationUser.FirtsName;
            user.LastName = applicationUser.LastName;
            user.Email = applicationUser.Email;
            user.Ward = applicationUser.Ward;
            user.City = applicationUser.City;
            user.District = applicationUser.District;

            if (ModelState.IsValid)
            {  
                if (applicationUser.PasswordHash != null)
                {
                    user.PasswordHash = _userManager.PasswordHasher.HashPassword(user, applicationUser.PasswordHash);
                }
                if (roleofuser == "ManagerUser")
                {
                    await _userManager.AddToRoleAsync(user, "ManagerUser");
                }
                else
                {
                    await _userManager.AddToRoleAsync(user, "User");
                }
                if (lockuser == "0Day")
                {
                    user.LockoutEnd = applicationUser.LockoutEnd;
                }
                if (lockuser == "3Day")
                {
                    user.LockoutEnd = DateTime.Now.AddDays(3);
                }
                if (lockuser == "1Week")
                {
                    user.LockoutEnd = DateTime.Now.AddDays(7);
                }
                if (lockuser == "Forever")
                {
                    user.LockoutEnabled = false;
                }
                if (lockuser == "UnLock")
                {
                    user.LockoutEnd = null;
                    user.LockoutEnabled = true;
                }
                if (_res.UpdateUser(user))
                {
                   return await Task.Run(() => RedirectToAction("Index"));
                }
               
            }
            
            return View(user);
        }
        [HttpPost, ActionName("DeleteUser")]
        public async Task<IActionResult> DeleteHouse(string id)
        {
            if (await _res.DeleteUser(id))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
