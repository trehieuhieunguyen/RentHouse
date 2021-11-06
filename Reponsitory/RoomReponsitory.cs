using Microsoft.EntityFrameworkCore;
using RentHouse.Data;
using RentHouse.Models;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Reponsitory
{
    public class RoomReponsitory : IRoomReponsitory
    {
        private readonly ApplicationDbContext _db;
        public RoomReponsitory(ApplicationDbContext db)
        {
            _db = db;
        }
        public bool SaveChange()
        {
            return _db.SaveChanges() > 0;
        }
        public bool CreateRoom(RoomHouse roomHouse)
        {
            _db.roomHouses.Add(roomHouse);
            return SaveChange();
        }
        public bool CheckHouseId(int id)
        {
            var x = _db.houses.Where(x => x.Id == id).ToList();
            if (x.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public async Task<bool> DeleteRoom(int id)
        {
            var x =await _db.roomHouses.FirstOrDefaultAsync(x => x.Id == id);
            if (x.StatusRent == true)
            {
                x.StatusRent = false;
            }
            else if(x.StatusRent == false)
            {
                x.StatusRent = true;
            }
            _db.roomHouses.Update(x);
            return SaveChange();
        }
        public bool CheckNumberRoom(int houseId,int numberRoom)
        {
            var x = _db.houses.Where(x => x.Id == houseId && x.AllRoom>numberRoom).ToList();
            if (x.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public async Task<ICollection<RoomHouse>> GetRoomHouseForUser(string title,string pricerent)
        {
            int price = 0;
            if (string.IsNullOrWhiteSpace(title)) title = "";
            if (pricerent == "0" || pricerent=="")
            {
                price = 10000;
            }
            if (pricerent == "10")
            {
                price = 10;
            }
            if (pricerent == "20")
            {
                price = 20;
            }
            if (pricerent == "50")
            {
                price = 50;
            }
            if (pricerent == "100")
            {
                price = 100;
            }
            var x =await _db.roomHouses.Include(x=>x.house).Where(x=>(x.house.City.Contains(title)
                                                ||x.house.District.Contains(title)
                                                ||x.house.Ward.Contains(title)
                                                ||x.house.NameHourse.Contains(title))
                                                &&x.PriceRent<=price).ToListAsync();
            return x;
        }
        public async Task<RoomHouse> GetRoomHouseForUserbyId(int id)
        {
            var x = await _db.roomHouses.Include(x=>x.house).FirstOrDefaultAsync(x=>x.Id==id);
            return x;
        }
        public async Task<IOrderedQueryable<RoomHouse>> GetRoomHouse(string title)
        {
            if (title == ""||title==null)
            {
                var x = _db.roomHouses.AsNoTracking().OrderBy(x => x.Id);
                return x;
            }
            else
            {
                var x = _db.roomHouses.AsNoTracking().Where(x => x.Id.ToString() == title).OrderBy(x => x.Id);
                return x;
            }  
        }
    }
}
