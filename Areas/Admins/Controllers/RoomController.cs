using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = "Admins")]
    public class RoomController : Controller
    {
        private readonly IRoomReponsitory _res;
        public RoomController(IRoomReponsitory res)
        {
            _res = res;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, [FromQuery(Name = ("input"))] string input = "")
        {
            ViewBag.KeywordRoom = input;
            var roomhouse = await _res.GetRoomHouse(ViewBag.KeywordRoom);
            var query = await PagingList<RoomHouse>.CreateAsync(roomhouse, 2, pageIndex);
            return View(query);
        }

        public async Task<IActionResult> CreateRoom()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomHouse roomHouse, IFormFile file)
        {
            if (file != null)
            {
                roomHouse.UrlImg = file.FileName;
            }
            if (!ModelState.IsValid)
            {
                if (!_res.CheckHouseId(roomHouse.HouseId))
                {
                    ModelState.AddModelError("HouseId", "Don't Have Exsits");
                    return View();
                }
                else
                {
                    if (!_res.CheckNumberRoom(roomHouse.HouseId, roomHouse.RoomNumber))
                    {
                        ModelState.AddModelError("RoomNumber", "Don't Have Exsits");
                        return View();
                    }
                    else
                    {
                        if (_res.CreateRoom(roomHouse))
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", roomHouse.UrlImg);
                            using (var fileSteam = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(fileSteam);
                            }
                            return RedirectToAction("Index");
                        }
                    }
                }

            }
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> DeleteRoom(int id)
        {
            if (await _res.DeleteRoom(id))
            {
                return RedirectToAction("Index");
            }
            return View();
        }
    }
}
