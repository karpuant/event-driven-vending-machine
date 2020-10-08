using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Net.Http;
using System.Threading.Tasks;
using VendingMachine.Web.Models;
using VendingMachine.Web.Services;

namespace VendingMachine.Web.Controllers
{
    public class HomeController : Controller
    {
        private readonly ILogger<HomeController> _logger;
        private readonly VendingService _vendingService;

        public HomeController(ILogger<HomeController> logger, VendingService vendingService)
        {
            _logger = logger;
            _vendingService = vendingService;
        }

        public async Task<IActionResult> Index()        
        {
            var model = new VendingMachineData();
            try
            {
                model.Products = (await _vendingService.GetAvailableProductsAsync()).Products;
                model.Denominations = _vendingService.GetDenominations();
                model.Deposit = (await _vendingService.GetDepositAsync()).TotalAmount;
            }
            catch (HttpRequestException e)
            {
                model.ErrorMessage = e.Message;
            }
            return View(model);
        }

        [HttpGet]
        public async Task<JsonResult> GetDeposit()
        {
            var deposit = await _vendingService.GetDepositAsync();
            return new JsonResult(deposit);
        }

        [HttpPut]
        public async Task<JsonResult> PutCoins([FromBody]PutCoinRequest request)
        {
            var deposit = await _vendingService.AcceptCoinsAsync(request.Denomination, request.Amount);
            return new JsonResult(deposit);
        }

        [HttpDelete]
        public async Task ReturnCoins()
        {
            await _vendingService.ReturnCoinsAsync();
        }

        [HttpPost]
        public async Task<JsonResult> Purchase([FromBody]ProductPurchaseRequest request)
        {
            var purchase = await _vendingService.Purchase(request.ProductName);
            return new JsonResult(purchase);
        }
    }
}
