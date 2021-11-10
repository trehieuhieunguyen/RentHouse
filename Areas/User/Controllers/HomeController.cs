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
        private readonly IHouseReponsitory _resHouse;
        public HomeController(IRoomReponsitory res, IHouseReponsitory resHouse)
        {
            _res = res;
            _resHouse = resHouse;
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
            ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(id);
            roomCommentVM.imageUploadOfRooms =  imageUploads;
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
                ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(id);
                roomCommentVM.imageUploadOfRooms = imageUploads;
                return View("DetailRoom", roomCommentVM);
               
            }
            return RedirectToAction("DetailRoom", id);
        }
        public async Task<IActionResult> DetailHouse(int id)
        {
            if (id == 0)
            {
                TempData["Error"] = "Not Found house,please refresh page";
                return BadRequest();
            }

            House house = await _resHouse.GetHouseById(id);
            if (house == null)
            {
                return NotFound();
            }
            ICollection<ImageUpload> ImageUpload = await _resHouse.GetSubImage(house.Id);
            HouseVM houseVM = new HouseVM();
            houseVM.imageUploads = ImageUpload;
            houseVM.house = house;
            return View(houseVM);
        }
    }
}
