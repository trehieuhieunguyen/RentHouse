using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using RentHouse.Models.ViewModel;
using RentHouse.Reponsitory.IReponsitory;

namespace RentHouse.Areas.Admins.Controllers
{
    [Area("Admins")]
    [Authorize(Roles = "Admins")]
    public class DashBoardController : Controller
    {
        
        private readonly IDashBoardReponsitory _res;

        public DashBoardController(IDashBoardReponsitory res)
        {
            _res = res;
        }

        public async Task<IActionResult> Index()
        {
            List<int> listCountHouseInMonth = new List<int>();
            for (int i = 1; i <= 12; i++)
            {
                var x = await _res.GetCountUserOneMonth(i);
                listCountHouseInMonth.Add(x);
            }
            ViewCountHouseVM viewCountHouseVM = new ViewCountHouseVM
            {
                getCountHouseInMonth = listCountHouseInMonth
            };
            return await Task.Run(() => View(viewCountHouseVM));
        }
    }
}
