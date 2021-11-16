using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
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

        public async Task<IActionResult> Index(int pageIndex = 1, [FromQuery(Name = ("input"))] string input = "", int selectpage = 4)
        {
            if (selectpage <= 0)
            {
                selectpage = 4;
            }
            ViewBag.KeywordRoom = input;
            ViewBag.pagechoose = selectpage;
            var roomhouse = await _res.GetRoomHouse(ViewBag.KeywordRoom);
            var query = await PagingList<RoomHouse>.CreateAsync(roomhouse, ViewBag.pagechoose, pageIndex);
            return View(query);
        }

        public async Task<IActionResult> CreateRoom()
        {
            return View();
        }
        [HttpPost]
        public async Task<IActionResult> CreateRoom(RoomHouse roomHouse, IFormFile file, IList<IFormFile> fileList)
        {
            if (file != null)
            {
                roomHouse.UrlImg = file.FileName;
            }
            roomHouse.UpdateTime= DateTime.Now;
            roomHouse.Star = 4;
            if (!ModelState.IsValid)
            {
                if (!_res.CheckNumberRoom(roomHouse.HouseId, roomHouse.RoomNumber))
                {
                    ModelState.AddModelError("RoomNumber", "Please Enter Number In All Room");
                    return View();
                }
                if (_res.CheckRoomNumberCreate(roomHouse.RoomNumber, roomHouse.HouseId) == false)
                {
                    ModelState.AddModelError("RoomNumber", "RoomNumber Had Exists");
                    return View();
                }
                if (!_res.CheckHouseId(roomHouse.HouseId))
                {
                    ModelState.AddModelError("HouseId", "Don't have exsits");
                    return View();
                }
                
                else
                {
                    
                    if (roomHouse.RoomSize <= 0)
                    {
                        ModelState.AddModelError("RoomSize", "Please Enter Number > 0");
                        return View();
                    }
                    if (roomHouse.Windowns < 0)
                    {
                        ModelState.AddModelError("Windowns", "Please Enter Number > 0");
                        return View();
                    }
                    if (roomHouse.PriceRent <= 0)
                    {
                        ModelState.AddModelError("PriceRent", "Please Enter Number > 0");
                        return View();
                    }
                    if (file == null)
                    {
                        ModelState.AddModelError("UrlImg", "Please Enter File Image");
                        return View();
                    }
                    else
                    {
                        if (_res.CreateRoom(roomHouse))
                        {
                            ImageUploadOfRoom imageUploadOfRoom = new ImageUploadOfRoom
                            {
                                RoomHouseId = roomHouse.Id,
                                Name = roomHouse.house.NameHourse
                            };
                            RoomVM roomVM = new RoomVM();

                            roomVM.formFiles = fileList;
                            roomVM.imageUploadOfRoom = imageUploadOfRoom;
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", roomHouse.UrlImg);
                            using (var fileSteam = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(fileSteam);
                            }
                            if (await UpdateSubImage(roomVM))
                            {
                                TempData["SuccessFull"] = "Create House SuccessFull";
                                return RedirectToAction("Index");
                            }
                        }
                    }
                }

            }
            return View();
        }
        public async Task<bool> UpdateSubImage(RoomVM roomVM)
        {
            IList<ImageUploadOfRoom> houseUpload = new List<ImageUploadOfRoom>();
            if (roomVM.formFiles != null)
            {
                foreach (IFormFile files in roomVM.formFiles)
                {

                    ImageUploadOfRoom houseUploadxx = new ImageUploadOfRoom();
                    houseUploadxx.Name = roomVM.imageUploadOfRoom.Name;
                    houseUploadxx.RoomHouseId = roomVM.imageUploadOfRoom.RoomHouseId;
                    houseUploadxx.Image = files.FileName;
                    var pathImage = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", houseUploadxx.Image);
                    using (var fileSteam = new FileStream(pathImage, FileMode.Create))
                    {
                        files.CopyTo(fileSteam);
                    }
                    houseUpload.Add(houseUploadxx);

                }

            }
            if (_res.CreateImageRoom(houseUpload))
            {
                return true;
            }
            return false;
        }
        public async Task<IActionResult> EditRoom(int id)
        {
            if (id == 0)
            {
                return BadRequest();
            }
            var roomHouse = await _res.GetRoomForAdmin(id);
            if (roomHouse == null)
            {
                return NotFound();
            }
            RoomVM roomVM = new RoomVM();
            ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(roomHouse.Id);
            roomVM.roomHouse = roomHouse;
            roomVM.imageUploadOfRooms = imageUploads;
            return await Task.Run(() => View(roomVM));

        }
        [HttpPost]
        public async Task<IActionResult> EditRoom(RoomVM roomVM, IList<IFormFile> files, IFormFile? file)
        {
            ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(roomVM.roomHouse.Id);
            List<ImageUploadOfRoom> imageUploadOfs = new List<ImageUploadOfRoom>();
            RoomHouse roomHouse = await _res.GetRoomForAdmin(roomVM.roomHouse.Id);
            roomHouse.Id = roomVM.roomHouse.Id;
            roomHouse.HouseId = roomVM.roomHouse.HouseId;
            roomHouse.PriceRent = roomVM.roomHouse.PriceRent;
            roomHouse.RoomNumber = roomVM.roomHouse.RoomNumber;
            roomHouse.RoomSize = roomVM.roomHouse.RoomSize;
            roomHouse.Windowns = roomVM.roomHouse.Windowns;
            roomHouse.StatusRent = roomVM.roomHouse.StatusRent;
            roomHouse.UpdateTime = DateTime.Now;
            if (!_res.CheckHouseId(roomHouse.HouseId))
            {
                ModelState.AddModelError("roomHouse.HouseId", "Don't Have Exsits");
                return View(roomVM);
            }
            if (_res.CheckRoomNumberEdit(roomHouse.RoomNumber, roomHouse.HouseId, roomHouse.Id) == false)
            {
                ModelState.AddModelError("roomHouse.RoomNumber", "RoomNumber had exists");
                return View(roomVM);
            }
            else
            {
                if (!_res.CheckNumberRoom(roomHouse.HouseId, roomHouse.RoomNumber))
                {
                    ModelState.AddModelError("roomHouse.RoomNumber", "Please Enter Number > 0");
                    return View(roomVM);
                }
                if (roomHouse.RoomSize <= 0)
                {
                    ModelState.AddModelError("roomHouse.RoomSize", "Please Enter Number > 0");
                    return View(roomVM);
                }
                if (roomHouse.Windowns < 0)
                {
                    ModelState.AddModelError("roomHouse.Windowns", "Please Enter Number > 0");
                    return View(roomVM);
                }
                if (roomHouse.PriceRent <= 0)
                {
                    ModelState.AddModelError("roomHouse.PriceRent", "Please Enter Number > 0");
                    return View(roomVM);
                }
                
            }
                
                if (file != null)
                {
                    roomHouse.UrlImg = file.FileName;
                }
                else
                {
                    roomHouse.UrlImg = roomVM.roomHouse.UrlImg;
                }
                roomVM.roomHouse = roomHouse;

                if (files.Count != 0)
                {
                    foreach (var filex in files)
                    {
                        ImageUploadOfRoom imageUpload = new ImageUploadOfRoom();
                        imageUpload.Image = filex.FileName;
                        imageUpload.RoomHouseId = roomVM.roomHouse.Id;
                        imageUpload.Name = "HadCheck";
                        imageUploadOfs.Add(imageUpload);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", filex.FileName);
                        using (var fileSteam = new FileStream(path, FileMode.Create))
                        {
                            filex.CopyTo(fileSteam);
                        }

                    }
                    _res.RemoveImgae(imageUploads);
                    _res.CreateImageRoom(imageUploadOfs);
                    roomVM.imageUploadOfRooms = imageUploadOfs;
                }
                else
                {
                    roomVM.imageUploadOfRooms = imageUploads;
                }
                if (!ModelState.IsValid)
                {

                    if (_res.EditRoom(roomHouse))
                    {
                        if (file != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", roomHouse.UrlImg);
                            using (var fileSteam = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(fileSteam);
                            }
                        }
                        TempData["SuccessFull"] = "Update House SuccessFull";
                        return RedirectToAction("Index");
                    }

                }

                return View(roomVM);
            }
            public async Task<IActionResult> DetailRoom(int id)
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                ICollection<ImageUploadOfRoom> imageUploads = await _res.imageUploadOfRooms(id);
                RoomHouse roomHouse = await _res.GetRoomForAdmin(id);
                if (roomHouse == null)
                {
                    return NotFound();
                }
                RoomVM roomVM = new RoomVM();
                roomVM.roomHouse = roomHouse;
                roomVM.imageUploadOfRooms = imageUploads;
                return View(roomVM);
            }
            [HttpPost]
            public async Task<IActionResult> DeleteRoom(int id)
            {
                if (id == 0)
                {
                    return BadRequest();
                }
                if (await _res.DeleteRoom(id))
                {
                    return RedirectToAction("Index");
                }
                return View();
            }
        }
    }
