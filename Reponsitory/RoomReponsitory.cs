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
        public bool CheckExistsStarOfUser(string id, int roomid)
        {
            var x = _db.RatingStars.Where(x=>x.RoomHouseId==roomid).ToList();
            if (x == null)
            {
                return true;
            }
            foreach(var item in x)
            {
                if (item.ApplicationUserId == id)
                {
                    return false;
                }

            }
            return true;
        }
        public int StarOfUser(string id, int roomid)
        {
            var x = _db.RatingStars.Where(x => x.RoomHouseId == roomid).ToList();
            if (x == null)
            {
                return 0;
            }
            foreach (var item in x)
            {
                if (item.ApplicationUserId == id)
                {
                    return item.Star;
                }

            }
            return 0;
        }
        public  async Task<double> StarOfRoom(int roomid)
        {
            var x =await _db.RatingStars.Where(x => x.RoomHouseId == roomid).ToListAsync();
            if (x.Count != 0)
            {
                return  x.Average(y => y.Star);
            }
            else
            {
                return  0;
            }
           
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
        public bool CreateStar(RatingStar rating)
        {
            _db.RatingStars.Add(rating);
            return SaveChange();
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
            var x = await _db.roomHouses.Include(x=>x.house).FirstOrDefaultAsync(x => x.Id == id);
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
        public async Task<ICollection<HistoryPay>> GetHistory(string Id)
        {
            var history = await _db.HistoryPay.Include(x => x.RoomHouse.house).Where(x => x.ApplicationUserId == Id).ToListAsync();
            return history;
        }
        public async Task<string> GetHouseOfUser(int houseId)
        {
            var userhouse = await _db.houseOfUser.Include(x=>x.ApplicationUser).FirstOrDefaultAsync(x=>x.HouseId==houseId);
            return userhouse.ApplicationUser.Email;
        }
        public bool CreatePayPal(HistoryPay historyPay)
        {
            _db.HistoryPay.Add(historyPay);
            return SaveChange();
        }
        public bool CheckRoomNumberChange(int roomnumber, int houseId)
        {

            var x = _db.roomHouses.Where(x => x.HouseId == houseId).ToList();
            foreach (var item in x)
            {
                if (item.RoomNumber == roomnumber)
                {
                    return false;
                }
            }
            return true;

        }
        public bool CheckRoomNumberHadRent(int roomnumber, int houseId)
        {

            var x = _db.roomHouses.Where(x => x.HouseId == houseId).ToList();
            foreach (var item in x)
            {
                if (item.RoomNumber == roomnumber && item.StatusRent == false)
                {
                    return true;
                }
            }
            return false;

        }
        public async Task<HistoryPay> GetHistoryForAdmin(int roomId)
        {
            var x = await _db.HistoryPay.Include(x=>x.RoomHouse).FirstOrDefaultAsync(x=>x.RoomHouseId==roomId);
            return x;
        }
        public bool EditHistory(HistoryPay historyPay)
        {
            _db.HistoryPay.Update(historyPay);
            return SaveChange();
        }
        public async Task<RoomHouse> GetRoomEdit(int houseId, int roomnumber)
        {
            var room = await _db.roomHouses.FirstOrDefaultAsync(x=>x.HouseId==houseId&& x.RoomNumber==roomnumber);
            return room;
        }
        public bool SaveMutilRoom(IList<RoomHouse> roomHouses)
        {
            _db.roomHouses.UpdateRange(roomHouses);
            return SaveChange();
        }
    }
}
