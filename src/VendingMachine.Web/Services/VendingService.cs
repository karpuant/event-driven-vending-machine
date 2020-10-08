using Newtonsoft.Json;
using System.Collections.Generic;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using VendingMachine.Web.Models;

namespace VendingMachine.Web.Services
{
    public class VendingService
    {
        private readonly HttpClient _httpClient;

        public VendingService(HttpClient client)
        {
            _httpClient = client;
        }

        public async Task<ProductList> GetAvailableProductsAsync()
        {
            var response = await _httpClient.GetAsync("products/");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<ProductList>(content);
        }

        public async Task<Deposit> AcceptCoinsAsync(int denomination, int amount)
        {
            var request = new StringContent(
                JsonConvert.SerializeObject(new { Denomination = denomination, Amount = amount }),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PutAsync("deposit/", request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Deposit>(content);
        }

        public async Task ReturnCoinsAsync()
        {
            var response = await _httpClient.DeleteAsync("deposit/");

            response.EnsureSuccessStatusCode();
        }

        public async Task<Deposit> GetDepositAsync()
        {
            var response = await _httpClient.GetAsync("deposit/");

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Deposit>(content);
        }

        public IEnumerable<Denomination> GetDenominations()
        {
            return _denominations;
        }

        public async Task<Purchase> Purchase(string productName)
        {
            var request = new StringContent(
                JsonConvert.SerializeObject(new ProductPurchaseRequest { ProductName = productName }),
                Encoding.UTF8,
                "application/json");
            var response = await _httpClient.PostAsync("products/", request);

            response.EnsureSuccessStatusCode();

            var content = await response.Content.ReadAsStringAsync();
            return JsonConvert.DeserializeObject<Purchase>(content);
        }



        private static List<Denomination> _denominations = new List<Denomination>
        {
            new Denomination
            {
                Value = 10,
                DiplayName = "10 cents"
            },
            new Denomination
            {
                Value = 20,
                DiplayName = "20 cents"
            },
            new Denomination
            {
                Value = 50,
                DiplayName = "50 cents"
            },
            new Denomination
            {
                Value = 100,
                DiplayName = "1 euro"
            }
        };
    }
}
