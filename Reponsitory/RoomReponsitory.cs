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
        public bool CreateImageRoom(IList<ImageUploadOfRoom> imageUploadOfRoom)
        {
            _db.SubImageOfRoom.AddRange(imageUploadOfRoom);
            return SaveChange();
        }
        public bool RemoveImgae(ICollection<ImageUploadOfRoom> imageUploadOfRoom)
        {
            _db.SubImageOfRoom.RemoveRange(imageUploadOfRoom);
            return SaveChange();
        }
        public bool EditRoom(RoomHouse roomhouse)
        {
            _db.roomHouses.Update(roomhouse);
            return SaveChange();
        }
        public async Task<ICollection<ImageUploadOfRoom>> imageUploadOfRooms(int id)
        {
            var x = await _db.SubImageOfRoom.Where(x => x.RoomHouseId == id).ToListAsync();
            return x;
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
            var x = await _db.roomHouses.FirstOrDefaultAsync(x => x.Id == id);
            if (x.StatusRent == true)
            {
                x.StatusRent = false;
            }
            else if (x.StatusRent == false)
            {
                x.StatusRent = true;
            }
            _db.roomHouses.Update(x);
            return SaveChange();
        }
        public bool CheckNumberRoom(int houseId, int numberRoom)
        {
            var x = _db.houses.Where(x => x.Id == houseId && x.AllRoom > numberRoom && numberRoom > 0).ToList();
            if (x.Count > 0)
            {
                return true;

            }
            else
            {
                return false;
            }
        }
        public bool CheckRoomNumberCreate(int roomnumber, int houseId)
        {

            var x = _db.roomHouses.Where(x => x.HouseId == houseId).ToList();
            foreach(var item in x)
            {
                if(item.RoomNumber == roomnumber)
                {
                    return false;
                }
            }
            return true;
          
        }
        public bool CheckRoomNumberEdit(int roomnumber, int houseId, int roomId)
        {

            var x = _db.roomHouses.Where(x => x.HouseId == houseId && x.Id != roomId).ToList();
            foreach (var item in x)
            {
                if (item.RoomNumber == roomnumber)
                {
                    return false;
                }
            }
            return true;
        }
        public async Task<IOrderedQueryable<RoomHouse>> GetRoomHouseForUser(string title, string pricerent)
        {
            int price = 0;
            if (string.IsNullOrWhiteSpace(title)) title = "";
            if (pricerent == "0" || pricerent == "")
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
            var x = _db.roomHouses.AsNoTracking().Include(x => x.house).Where(x => (x.house.City.Contains(title)
                                                  || x.house.District.Contains(title)
                                                  || x.house.Ward.Contains(title)
                                                  || x.house.NameHourse.Contains(title))
                                                  && x.PriceRent <= price && x.house.isDeleted == false && x.StatusRent == false).OrderByDescending(x => x.UpdateTime);
            return x;
        }
        public async Task<RoomHouse> GetRoomForAdmin(int id)
        {
            var x = await _db.roomHouses.FirstOrDefaultAsync(x => x.Id == id);
            return x;
        }
        
        public async Task<ICollection<MessageRoom>> GetCommentRooms(int id)
        {
            var x = await _db.messageRooms.Include(x => x.ApplicationUser).Include(x => x.RoomHouse).Where(x => x.RoomHouseId == id).ToListAsync();
            return x;
        }

        public bool SaveComment(MessageRoom comment)
        {
            var x = _db.messageRooms.Update(comment);
            return SaveChange();
        }
        public async Task<RoomHouse> GetRoomHouseForUserbyId(int id)
        {
            var x = await _db.roomHouses.Include(x => x.house).FirstOrDefaultAsync(x => x.Id == id);
            return x;
        }
        public async Task<IOrderedQueryable<RoomHouse>> GetRoomHouse(string title)
        {
            if (title == "" || title == null)
            {
                var x = _db.roomHouses.AsNoTracking().Include(x => x.house).OrderByDescending(x => x.UpdateTime);
                return x;
            }
            else
            {
                var x = _db.roomHouses.AsNoTracking().Include(x => x.house).Where(x => (x.house.City.Contains(title)
                                                || x.house.District.Contains(title)
                                                || x.house.Ward.Contains(title)
                                                || x.house.NameHourse.Contains(title)
                                                || x.Id.ToString() == title)).OrderByDescending(x => x.UpdateTime);
                return x;
            }
        }
    }
}
