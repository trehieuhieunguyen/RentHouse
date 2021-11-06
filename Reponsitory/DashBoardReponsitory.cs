using Microsoft.EntityFrameworkCore;
using RentHouse.Data;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Reponsitory
{
    public class DashBoardReponsitory : IDashBoardReponsitory
    {
        private readonly ApplicationDbContext _db;

        public DashBoardReponsitory(ApplicationDbContext db)
        {
            _db = db;
        }

        public async Task<int> GetCountUserOneMonth(int i)
        {
            var currYear = DateTime.Now.Year;

            var firstOfThisMonth = new DateTime(currYear, i, 1);
            var firstOfNextMonth = firstOfThisMonth.AddMonths(1);

            //totalSMS =await (from x in _db.Auctions
            //                where x.Time_End >= firstOfThisMonth && x.Time_Start < firstOfNextMonth
            //                select x).CountAsync();
            var totalSMS = await _db.houses.Where(x => x.TimeCreate >= firstOfThisMonth && x.TimeCreate < firstOfNextMonth).CountAsync();

            return totalSMS;
        }
    }
}
