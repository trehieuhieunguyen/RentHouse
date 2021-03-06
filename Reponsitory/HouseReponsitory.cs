using Microsoft.EntityFrameworkCore;
using RentHouse.Data;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Reponsitory
{
    public class HouseReponsitory : IHouseReponsitory
    {
        private readonly ApplicationDbContext _db;
        public HouseReponsitory(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<bool> CreateHouse(House house)
        {
            _db.houses.Add(house);
            return await SaveChange();
        }
        public async Task<bool> CreateHouseOfUser(HouseOfUser houseOfUser)
        {
            _db.houseOfUser.Add(houseOfUser);
            return await SaveChange();
        }
        public async Task<bool> CreateImgae(IList<ImageUpload> imageUpload)
        {
            _db.imageUploads.AddRange(imageUpload);
            return await SaveChange();
        }
        public async Task<bool> RemoveImgae(ICollection<ImageUpload> imageUpload)
        {
            _db.imageUploads.RemoveRange(imageUpload);  
            return await SaveChange();
        }
     
        public async Task<bool> Delete(int id)
        {
            
            House house = _db.houses.FirstOrDefault(x => x.Id == id);
            if(house == null)
            {
                return false;
            }
            if (house.isDeleted == false)
            {
                house.isDeleted = true;
                house.TimeUpdate = DateTime.Now;
            }
            else
            {
                house.isDeleted = false;
                house.TimeUpdate = DateTime.Now;
            }
            return await SaveChange();
        }

        public async Task<bool> EditHouse(House house)
        {
            _db.houses.Update(house);
            return await SaveChange();
        }

        public async Task<IOrderedQueryable<House>> GetHouse(string title)
        {
            if (string.IsNullOrWhiteSpace(title)) title = "";

            var house =  _db.houses.AsNoTracking().Where(x => x.NameHourse.Contains(title)||x.Id.ToString()==title).OrderByDescending(x => x.TimeUpdate);
            return house;
        }
        public async Task<bool> CheckNameHouse(string Name,int Id)
        {
            List< House > house  = await _db.houses.ToListAsync();
            foreach(var houses in house)
            {
                if (Id != houses.Id)
                {
                    if (houses.NameHourse == Name){
                        return false;
                    }
                }
            }
            return true;    
        }
        public async Task<bool> CheckNameCreateHouse(string Name)
        {
            List<House> house = await _db.houses.ToListAsync();
            foreach (var houses in house)
            {
               
                    if (houses.NameHourse == Name)
                    {
                        return false;
                    }
            }
            return true;
        }
        public async Task<House> GetHouseById(int id)
        {
            return await _db.houses.FirstOrDefaultAsync(x => x.Id == id);
        }
        public async Task<ICollection<ImageUpload>> GetSubImage(int id)
        {
            return await _db.imageUploads.Where(x => x.HouseId == id).ToListAsync();
        }
        public async Task<bool> SaveChange()
        {
            return await _db.SaveChangesAsync()>0;
        }
        public async Task<ICollection<RoomHouse>> getRoomInHouse(int id)
        {
            var x =await _db.roomHouses.Include(x => x.house).Where(x=>x.HouseId == id).ToListAsync();
            return x;
        }
        public async Task<ICollection<House>> GetHouseForUser(string id)
        {
            var x = await _db.houseOfUser.Where(x=>x.ApplicationUserId==id).ToListAsync();
            ICollection<House> hou = new List<House>();
            foreach(var houses in x)
            {
                var house = await _db.houses.FirstOrDefaultAsync(x => x.Id == houses.HouseId);
                hou.Add(house);
            }
            return hou;
        }
        public async Task<ICollection<House>> getConfirmHouse()
        {
            var x = await _db.houses.Where(x => x.ConfirmHouse == true).ToListAsync();
            return x;
        }
    }
}
