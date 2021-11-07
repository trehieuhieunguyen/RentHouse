using Microsoft.AspNetCore.Mvc;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;
using System.Diagnostics;

namespace RentHouse.Controllers
{
    public class HomeController : Controller
    {
        private readonly IRoomReponsitory _res;

        public HomeController(IRoomReponsitory res)
        {
            _res = res;
        }

        public async Task<IActionResult> Index([FromQuery(Name = ("input"))] string input = "", string priceRent = "")
        {
            var x = await _res.GetRoomHouseForUser(input, priceRent);
            return View(x);
        }
        
        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}