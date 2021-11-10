﻿using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using ReflectionIT.Mvc.Paging;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles ="Admins")]
    public class HomeController : Controller
    {       

        private readonly IHouseReponsitory _res;
        public HomeController(IHouseReponsitory res)
        {
            _res = res;
        }

        public async Task<IActionResult> Index(int pageIndex = 1, [FromQuery(Name = ("input"))] string input = "", int selectpage=4)

        {
            if (selectpage <= 0)
            {
                selectpage = 4;
            }
            ViewBag.keyword = input;
            ViewBag.pagechoose = selectpage;
            var houses = await _res.GetHouse(ViewBag.keyword);
            var query = await PagingList<House>.CreateAsync(houses, ViewBag.pagechoose, pageIndex);
            return View(query);
        }
        public async Task<IActionResult> CreateHouse()
        {         
            return await Task.Run(()=> View()) ;
        }
        
        [HttpPost]
        public async Task<IActionResult> CreateHouse(House house,IFormFile file, IList<IFormFile> fileList)
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
                if (await _res.CreateHouse(house))
                {
                    HouseVM houseVM = new HouseVM();
                        ImageUpload houseUpload = new ImageUpload()
                        {
                            HouseId= house.Id,
                            Name = house.NameHourse
                        };
                    houseVM.formFiles = fileList;
                    houseVM.imageUpload = houseUpload;
                    var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", house.ImgUrl);
                    using (var fileSteam = new FileStream(path, FileMode.Create))
                    {
                        file.CopyTo(fileSteam);
                    }
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
           if(await _res.CreateImgae(houseUpload))
            {
                return true;
            }
            return false;
        }
        public async Task<IActionResult> EditHouse(string id)
        {
            if (id == null||int.TryParse(id,out int ds)==false)
            {
                return BadRequest();
            }
            House house = await _res.GetHouseById(int.Parse(id));
            if (house==null)
            {
                return NotFound();
            }
            HouseVM houseVM = new HouseVM();
            ICollection<ImageUpload> ImageUpload = await _res.GetSubImage(house.Id);
            houseVM.house = house;
            houseVM.imageUploads = ImageUpload;
            
            return View(houseVM);
        }
        [HttpPost]
        public async Task<IActionResult> EditHouse(  HouseVM housevm, IList<IFormFile> files, IFormFile? file)
        {
            ICollection<ImageUpload> ImageUploads = await _res.GetSubImage(housevm.house.Id);
            IList<ImageUpload> houseUpload = new List<ImageUpload>();
            House hou = await _res.GetHouseById(housevm.house.Id);
            hou.NameHourse = housevm.house.NameHourse;
            hou.District = housevm.house.District;
            hou.Ward = housevm.house.Ward;
            hou.City = housevm.house.City;
            if (housevm.house.AllRoom <= 0)
            {
                ModelState.AddModelError("house.AllRoom", "Please Enter Number > 0");
                return View(housevm);
            }
            hou.AllRoom = housevm.house.AllRoom;
            hou.TimeUpdate = DateTime.Now;
            if (file != null)
            {
                hou.ImgUrl = file.FileName;
            }
            else
            {
                hou.ImgUrl = housevm.house.ImgUrl;
            }
            housevm.house = hou;
            if (files.Count != 0)
            {
                    foreach (var filex in files)
                    {
                        ImageUpload houseUploadxx = new ImageUpload();
                        houseUploadxx.Image = filex.FileName;
                        houseUploadxx.HouseId = housevm.house.Id;
                        houseUploadxx.Name = housevm.house.NameHourse;
                        houseUpload.Add(houseUploadxx);
                        var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", filex.FileName);
                        using (var fileSteam = new FileStream(path, FileMode.Create))
                        {
                            filex.CopyTo(fileSteam);
                        }

                    }  
                await _res.RemoveImgae(ImageUploads);
                await _res.CreateImgae(houseUpload);
                housevm.imageUploads = houseUpload;
            }
            else
            {
                housevm.imageUploads = ImageUploads;
            }
            if (!ModelState.IsValid)
            {
                if (await _res.CheckNameHouse(housevm.house.NameHourse, housevm.house.Id) == false)
                {
                    ModelState.AddModelError("house.NameHourse", "Already Exist");
                }
                else
                {

                    if (await _res.EditHouse(hou))
                    {
                        if (file != null)
                        {
                            var path = Path.Combine(Directory.GetCurrentDirectory(), "wwwroot/img", hou.ImgUrl);
                            using (var fileSteam = new FileStream(path, FileMode.Create))
                            {
                                file.CopyTo(fileSteam);
                            }
                        }
                        TempData["SuccessFull"] = "Update House SuccessFull";
                        return RedirectToAction("Index");
                    }
                }
            }

            return View(housevm);
        }
        [HttpPost,ActionName("DeleteHouse")]
        public async Task<IActionResult> DeleteHouse(int id)
        {
            if(id == 0)
            {
                return BadRequest();
            }
            if (await _res.Delete(id))
            {
                TempData["SuccessFull"] = "Update House SuccessFull";
                return RedirectToAction("Index");
            }
            return View();
        }
        public async Task<IActionResult> DetailHouse(int id)
        {
            if (id == 0)
            {
                TempData["Error"] = "Not Found house,please refresh page";
                return BadRequest();
            }
            
            House house = await _res.GetHouseById(id);
            if(house == null)
            {
                return NotFound();
            }
            ICollection<ImageUpload> ImageUpload = await _res.GetSubImage(house.Id);
            HouseVM houseVM = new HouseVM();
            houseVM.imageUploads = ImageUpload;
            houseVM.house = house;
            return View(houseVM);
        }
        
    }
}
