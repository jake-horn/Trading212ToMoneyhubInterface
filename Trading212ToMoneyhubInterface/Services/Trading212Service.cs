using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using Trading212ToMoneyhubInterface.Models;

namespace Trading212ToMoneyhubInterface.Services
{
    public class Trading212Service : ITrading212Service
    {
        private readonly IHttpClientFactory _client;
        private readonly ILogger<Trading212Service> _logger; 

        public Trading212Service(IHttpClientFactory client, ILogger<Trading212Service> logger)
        {
            _client = client;
            _logger = logger;
        }

        public async Task<decimal> GetPortfolioBalance()
        {
            using HttpClient client = _client.CreateClient("Trading212Client" ?? throw new Exception("Trading212Client not found for CreateClient()"));
            _logger.LogInformation("HttpClient initialised successfully.");

            try
            {
                var balanceValue = await client.GetAsync("equity/account/cash");

                if (balanceValue.IsSuccessStatusCode)
                {
                    string responseBody = await balanceValue.Content.ReadAsStringAsync();
                    var totalValue = JsonSerializer.Deserialize<Trading212>(responseBody);

                    _logger.LogInformation("Balance retrieved successfully: {balance}", totalValue.Total);

                    return totalValue is null ? throw new NullReferenceException("Trading212 value is null") : totalValue.Total;
                }
            }
            catch
            {
                throw new Exception("Error");
            }

            throw new Exception("Error");
        }
    }
}
