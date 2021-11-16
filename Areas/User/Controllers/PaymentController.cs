using BraintreeHttp;
using Microsoft.AspNetCore.Mvc;
using PayPal.Core;
using PayPal.v1.Payments;
using RentHouse.Data;
using RentHouse.Extensions;
using RentHouse.Models;
using RentHouse.Models.ViewModel;
using RentHouse.Reponsitory.IReponsitory;
using System.Globalization;

namespace RentHouse.Areas.User.Controllers
{
    [Area("User")]
    public class PaymentController : Controller
    {
        private readonly ApplicationDbContext _db;
        private readonly string _clientId;
        private readonly string _secretKey;
        private readonly IRoomReponsitory _res;
        public PaymentController(IConfiguration configuration, ApplicationDbContext db, IRoomReponsitory res)
        {

            _clientId = configuration["Paypal:ClientId"];
            _secretKey = configuration["Paypal:SecretKey"];
            _db = db;
            _res = res;
        }
        private PayPalHttpClient Client()
        {
            var environment = new SandboxEnvironment(_clientId, _secretKey);
            var client = new PayPalHttpClient(environment);
            return client;
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> PaypalCheckout(int id)
        {
            RoomHouse room = _db.roomHouses.FirstOrDefault(x=>x.Id==id);
           
            var client = Client();

            var itemList = new ItemList()
            {
                Items = new List<Item>()
            };
            itemList.Items.Add(new Item()
            {
                Name = room.RoomNumber.ToString(),
                Currency = "USD",
                Price = room.PriceRent.ToString(CultureInfo.InvariantCulture),
                Quantity = 1.ToString(),
                Sku = "sku",
                Tax = "0"
            });
       
            var hostname = $"{HttpContext.Request.Scheme}://{HttpContext.Request.Host}";
            var paypalOrderId = Guid.NewGuid();

            var payment = new Payment()
            {
                Intent = "sale",
                Transactions = new List<PayPal.v1.Payments.Transaction>()
                {
                    new PayPal.v1.Payments.Transaction()
                    {
                        Amount = new Amount()
                        {
                            Total = room.PriceRent.ToString(CultureInfo.InvariantCulture),
                            Currency = "USD"
                        },
                        ItemList = itemList,
                        Description = $"Invoice #{paypalOrderId}",
                        InvoiceNumber = paypalOrderId.ToString()
                    }
                },
                RedirectUrls = new RedirectUrls()
                {
                    CancelUrl = $"{hostname}/Payment/Fail?orderId={paypalOrderId}",
                    ReturnUrl = $"{hostname}/User/Payment/PaypalSuccess?orderId={paypalOrderId}&roomId={id}"
                },
                Payer = new Payer()
                {
                    PaymentMethod = "paypal"
                }
            };


            PaymentCreateRequest request = new PaymentCreateRequest();
            request.RequestBody(payment);

            try
            {
                var response = await client.Execute(request);
                var statusCode = response.StatusCode;
                Payment result = response.Result<Payment>();

                var links = result.Links.GetEnumerator();
                string paypalRedirectUrl = null;
                while (links.MoveNext())
                {
                    LinkDescriptionObject lnk = links.Current;
                    if (lnk.Rel.ToLower().Trim().Equals("approval_url"))
                    {
                        //saving the payapalredirect URL to which user will be redirected for payment  
                        paypalRedirectUrl = lnk.Href;
                    }
                }

                return Redirect(paypalRedirectUrl);
            }
            catch (HttpException httpException)
            {
                var statusCode = httpException.StatusCode;
                var debugId = httpException.Headers.GetValues("PayPal-Debug-Id").FirstOrDefault();

                //Process when Checkout with Paypal fails
                return Redirect("/Customers/Payment/Fail");
            }

        }
        public async Task<IActionResult> PaypalSuccess([FromQuery(Name = "orderId")] string orderId, [FromQuery(Name = "paymentId")] string paymentId, [FromQuery(Name = "PayerID")] string PayerID, [FromQuery(Name = "roomId")] string roomId)
        {
           
            var request = new PaymentExecuteRequest(paymentId);

            request.RequestBody(new PaymentExecution()
            {
                PayerId = PayerID
            });


            var client = Client();

            var response = await client.Execute(request);
            var payment = response.Result<Payment>();

            //if (payment.State != "approved")
            //{ // no blance or something else?
            //    return RedirectToAction(nameof(Fail));
            //}
            var room =  await _res.GetRoomForAdmin(int.Parse(roomId));
            HistoryPay historyPay = new HistoryPay();
            historyPay.TimePay = DateTime.Now;
            historyPay.ApplicationUserId = User.GetUserId();
            historyPay.RoomHouseId = room.Id;
            historyPay.PricePay = room.PriceRent;
            _res.CreatePayPal(historyPay);
            room.StatusRent = true;
            _res.EditRoom(room);
            var items = new List<Item>();

            foreach (var cartitem in payment.Transactions)
            {
                foreach (var itemlist in cartitem.ItemList.Items)
                {
                    items.Add(new Item()
                    {
                        Name = itemlist.Name,
                        Price = itemlist.Price,
                        Quantity = itemlist.Quantity
                    });
                }
            }
            List<HistoryUserVM> historyUserVMs = new List<HistoryUserVM>();
            var result = await _res.GetHistory(User.GetUserId());
            foreach (var item in result)
            {
                HistoryUserVM historyUserVM = new HistoryUserVM();
                historyUserVM.historyPays = item;
                historyUserVM.EmailUser = await _res.GetHouseOfUser(item.RoomHouse.HouseId);
                historyUserVMs.Add(historyUserVM);
            }
            TempData["SuccessFull"] = "Rent Room By PayPal SuccessFull";
            return await Task.Run(() => RedirectToAction("GetHistoryPay", "Home", historyUserVMs));
            //return await Task.Run(() => RedirectToAction("Index", "Home", new { id = orderId }));
        }
    }
}
