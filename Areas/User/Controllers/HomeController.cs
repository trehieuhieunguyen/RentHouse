using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentHouse.Extensions;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
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
            RoomCommentVM roomCommentVM = new RoomCommentVM();
            
            var roomhouse = await _res.GetRoomHouseForUserbyId(id);
            roomCommentVM.roomHouse = roomhouse;
            
            var commentroom = await _res.GetCommentRooms(id);
            roomCommentVM.commentRooms = commentroom;
            
            return View(roomCommentVM);
        }
        [HttpPost]
        public async Task<IActionResult> CommitMessage(string message,int id)
        {
            MessageRoom commentRoom = new MessageRoom();
            commentRoom.ApplicationUserId = User.GetUserId();
            commentRoom.Status = true;
            commentRoom.UpdateTime= DateTime.Now;
            commentRoom.CreateTime= DateTime.Now;
            commentRoom.RoomHouseId = id;
            commentRoom.Text = message;
            if (_res.SaveComment(commentRoom))
            {
                RoomCommentVM roomCommentVM = new RoomCommentVM();

                var roomhouse = await _res.GetRoomHouseForUserbyId(id);
                roomCommentVM.roomHouse = roomhouse;

                var commentroom = await _res.GetCommentRooms(id);
                roomCommentVM.commentRooms = commentroom;

                return View("DetailRoom", roomCommentVM);
               
            }
            return RedirectToAction("DetailRoom", id);
        }
    }
}
