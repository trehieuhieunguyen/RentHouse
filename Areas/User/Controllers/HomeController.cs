using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User")]
    public class HomeController : Controller
    {
        private readonly IRoomReponsitory _res;

        public HomeController(IRoomReponsitory res)
        {
            _res = res;
        }

        public async Task<IActionResult> Index( [FromQuery(Name = ("input"))] string input = "", string priceRent="")
        {
            var x =await _res.GetRoomHouseForUser(input,priceRent);
            return View(x);
        }
        public async Task<IActionResult> DetailRoom(int id)
        {
            var house = await _res.GetRoomHouseForUserbyId(id);
            
            return View(house);
        }
    }
}
