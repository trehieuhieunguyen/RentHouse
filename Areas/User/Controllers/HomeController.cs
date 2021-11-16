using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using RentHouse.Extensions;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.User.Controllers
{
    [Area("User")]
    [Authorize(Roles = "User,Admins")]
    public class HomeController : Controller
    {
        private readonly IRoomReponsitory _res;
        private readonly IHouseReponsitory _resHouse;
        public HomeController(IRoomReponsitory res, IHouseReponsitory resHouse)
        {
            _res = res;
            _resHouse = resHouse;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, [FromQuery(Name = ("input"))] string input = "", string priceRent = "", int selectpage = 6)
        {
            ViewBag.keyword = input;
            ViewBag.pagechoose = selectpage;
            var houses = await _res.GetRoomHouseForUser(ViewBag.keyword, priceRent);
            var query = await PagingList<RoomHouse>.CreateAsync(houses, ViewBag.pagechoose, pageIndex);
            return View(query);
        }
        public async Task<IActionResult> DetailRoom(int id)
        {
            RoomCommentVM roomCommentVM = new RoomCommentVM();
            
            var roomhouse = await _res.GetRoomHouseForUserbyId(id);
            roomCommentVM.roomHouse = roomhouse;
            
            var commentroom = await _res.GetCommentRooms(id);
            roomCommentVM.commentRooms = commentroom;
            roomCommentVM.RatingStarUser = _res.CheckExistsStarOfUser(User.GetUserId(), roomhouse.Id);
            if(_res.StarOfUser(User.GetUserId(), roomhouse.Id) != 0)
            {
                roomCommentVM.StarUser = _res.StarOfUser(User.GetUserId(), roomhouse.Id);
            }
            else
            {
                roomCommentVM.StarUser = 0;
            }
            
            ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(id);
            roomCommentVM.imageUploadOfRooms =  imageUploads;
            return View(roomCommentVM);
        }
        [HttpPost]
        public async Task<IActionResult> CommitMessage(string message,int id,string ratingstar)
        {
            
            MessageRoom commentRoom = new MessageRoom();
            commentRoom.ApplicationUserId = User.GetUserId();
            commentRoom.Status = true;
            commentRoom.UpdateTime= DateTime.Now;
            commentRoom.CreateTime= DateTime.Now;
            commentRoom.RoomHouseId = id;
            commentRoom.Text = message;
            
            if (ratingstar != null && ratingstar!="")
            {
                RatingStar ratingStar = new RatingStar();
                ratingStar.Star = int.Parse(ratingstar);
                ratingStar.RoomHouseId = id;
                ratingStar.ApplicationUserId = User.GetUserId();
                ratingStar.CreateTime = DateTime.Now;
                _res.CreateStar(ratingStar);
            }
            
            if (_res.SaveComment(commentRoom))
            {
                RoomCommentVM roomCommentVM = new RoomCommentVM();

                var roomhouse = await _res.GetRoomHouseForUserbyId(id);
                var star = await _res.StarOfRoom(id);
                roomhouse.Star = star;
                _res.EditRoom(roomhouse);
                roomCommentVM.roomHouse = roomhouse;
                roomCommentVM.RatingStarUser = _res.CheckExistsStarOfUser(User.GetUserId(), roomhouse.Id);
                if (_res.StarOfUser(User.GetUserId(), roomhouse.Id) != 0)
                {
                    roomCommentVM.StarUser = _res.StarOfUser(User.GetUserId(), roomhouse.Id);
                }
                else
                {
                    roomCommentVM.StarUser = 0;
                }
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
        public async Task<IActionResult> CreateHouseForUser()
        {
            return await Task.Run(() => View());
        }

        [HttpPost]
        public async Task<IActionResult> CreateHouseForUser(House house, IFormFile file, IList<IFormFile> fileList)
        {
            if (file != null)
            {
                house.ImgUrl = file.FileName;
            }
            house.TimeCreate = DateTime.Now;
            house.TimeUpdate = DateTime.Now;
            if (ModelState.IsValid)
            {
                //if (house.District.Length < 5)
                //{
                //    ModelState.AddModelError("NameHourse", "Please Enter Length > 5");
                //    return View(house);
                //}
                if (house.AllRoom <= 0)
                {
                    ModelState.AddModelError("AllRoom", "Please Enter Number > 0");
                    return View();
                }
                if (await _resHouse.CreateHouse(house))
                {
                    HouseVM houseVM = new HouseVM();
                    ImageUpload houseUpload = new ImageUpload()
                    {
                        HouseId = house.Id,
                        Name = house.NameHourse
                    };
                    houseVM.formFiles = fileList;
                    houseVM.imageUpload = houseUpload;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", house.ImgUrl);
                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileSteam);
                    }
                    HouseOfUser houseOfUser = new HouseOfUser();
                    houseOfUser.HouseId = houseUpload.HouseId;
                    houseOfUser.ApplicationUserId = User.GetUserId();
                    await _resHouse.CreateHouseOfUser(houseOfUser);
                    if (await UpdateSubImage(houseVM))
                    {
                        TempData["SuccessFull"] = "Create House SuccessFull";
                        return RedirectToAction("Index");
                    }
                }
            }
            return View();
        }
        public async Task<bool> UpdateSubImage(HouseVM houseVM)
        {
            IList<ImageUpload> houseUpload = new List<ImageUpload>();
            if (houseVM.formFiles != null)
            {
                foreach (IFormFile files in houseVM.formFiles)
                {

                    ImageUpload houseUploadxx = new ImageUpload();
                    houseUploadxx.Name = houseVM.imageUpload.Name;
                    houseUploadxx.HouseId = houseVM.imageUpload.HouseId;
                    houseUploadxx.Image = files.FileName;
                    var pathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", houseUploadxx.Image);
                    using (var fileSteam = new FileStream(pathImage, FileMode.Create))
                    {
                        files.CopyTo(fileSteam);
                    }
                    houseUpload.Add(houseUploadxx);

                }

            }
            if (await _resHouse.CreateImgae(houseUpload))
            {
                return true;
            }
            return false;
        }
        public async Task<IActionResult> GetHouseOfUser()
        {
            var user = User.GetUserId();
            var houses = await _resHouse.GetHouseForUser(user);
            if(houses != null)
            {
                return View(houses);
            }
            else
            {
                return View();
            }
        }
        public async Task<IActionResult> GetHistoryPay()
        {
            List<HistoryUserVM> historyUserVMs = new List<HistoryUserVM>();
            var result =await _res.GetHistory(User.GetUserId());
            foreach(var item in result)
            {
                HistoryUserVM historyUserVM = new HistoryUserVM();
                historyUserVM.historyPays = item;
                historyUserVM.EmailUser = await _res.GetHouseOfUser(item.RoomHouse.HouseId);
                historyUserVMs.Add(historyUserVM);
            }
            
            return View(historyUserVMs);
        }
    }
}
