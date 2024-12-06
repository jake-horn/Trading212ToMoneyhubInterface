using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using PuppeteerSharp;

namespace Trading212ToMoneyhubInterface.Services
{
    public class MoneyhubService : IMoneyhubService
    {
        private readonly IConfiguration _configuration;
        private readonly ILogger<MoneyhubService> _logger; 

        public MoneyhubService(IConfiguration configuration, ILogger<MoneyhubService> logger)
        {
            _configuration = configuration;
            _logger = logger;
        }

        public async Task UpdateMoneyhub(string balanceToUpdate)
        {
            var trading212Key = _configuration.GetSection("MoneyHub:Trading212AccountKey").Value;

            // Arrange the browser and tab
            var browserFetcher = new BrowserFetcher();
            await browserFetcher.DownloadAsync();
            await using var browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            await using var page = await browser.NewPageAsync();
            _logger.LogInformation("Opened browser."); 

            // Go to MoneyHub and log in
            await page.GoToAsync($"https://client.moneyhub.co.uk/#accounts/details/{trading212Key}");
            await page.TypeAsync("#email", _configuration.GetSection("MoneyHub:Username").Value);
            await page.TypeAsync("#password", _configuration.GetSection("MoneyHub:Password").Value);
            _logger.LogInformation("Logged in.");

            // Click to update
            await page.ClickAsync(".sc-bxivhb.sc-ifAKCX.byYfdZ");
            await page.WaitForSelectorAsync("button[aria-label='Edit Account']");

            // Click to edit the account manually
            await page.ClickAsync("button[aria-label='Edit Account']");
            await page.WaitForSelectorAsync("button[class='sc-EHOje dtpHoS'] span[class='sc-bxivhb sc-ifAKCX byYfdZ']");

            // Click to edit the balance
            await page.ClickAsync("button[class='sc-EHOje dtpHoS'] span[class='sc-bxivhb sc-ifAKCX byYfdZ']");
            await page.WaitForSelectorAsync("#balance");

            // Get the balance value and delete each character
            var balanceValue = await page.EvaluateExpressionAsync<string>("document.querySelector('#balance').value");

            for (int i = 0; i < balanceValue.Length; i++)
            {
                await page.FocusAsync("#balance");
                await page.Keyboard.PressAsync("Backspace");
            }

            _logger.LogInformation("Deleted existing value."); 

            // Update balance and submit
            await page.TypeAsync("#balance", balanceToUpdate);

            await page.ClickAsync("button[type='submit']");
            await page.WaitForSelectorAsync(".sc-EHOje.jtVGey");

            // Save
            await page.ClickAsync(".sc-EHOje.jtVGey");

            _logger.LogInformation("Saved new value.");

            // Close tab and browser
            await page.DisposeAsync();
            await browser.DisposeAsync();

            _logger.LogInformation("Page and browser disposed.");
        }
    }
}
