using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;
using Trading212ToMoneyhubInterface.Services;

namespace Trading212ToMoneyhubInterface
{
    public class App : IApp
    {
        private readonly ITrading212Service _trading212Service;
        private readonly IMoneyhubService _moneyHubService;
        private readonly ILogger<App> _logger; 

        public App(ITrading212Service trading212, IMoneyhubService moneyHubService, ILogger<App> logger)
        {
            _trading212Service = trading212;
            _moneyHubService = moneyHubService;
            _logger = logger;
        }

        public async Task Run()
        {
            try
            {
                _logger.LogInformation("Starting Trading212ToMoneyhubInterface at {Now}.", DateTime.Now); 

                var currentBalance = await _trading212Service.GetPortfolioBalance();
                await _moneyHubService.UpdateMoneyhub(currentBalance.ToString());

                _logger.LogInformation("Successfully updated Trading212 value to {balance}", currentBalance.ToString());
            }
            catch (Exception ex)
            {
                _logger.LogInformation("Error running application: {Message}", ex.Message);
            }
        }
    }
}
