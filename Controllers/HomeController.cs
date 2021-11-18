using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
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

        public async Task<IActionResult> Index(int pageIndex = 1,[FromQuery(Name = ("input"))] string input = "", string priceRent = "", int selectpage = 6)
        {
          
            ViewBag.keyword = input;
            ViewBag.pagechoose = selectpage;
            var houses = await _res.GetRoomHouseForUser(ViewBag.keyword,priceRent);
            var query = await PagingList<RoomHouse>.CreateAsync(houses, ViewBag.pagechoose, pageIndex);
            return View(query);
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