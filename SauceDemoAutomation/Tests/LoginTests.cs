using Xunit;
using Microsoft.Playwright;
using SauceDemoAutomation.PageObjects;
using System.Threading.Tasks;
using Allure.Xunit.Attributes; // For Allure

namespace SauceDemoAutomation.Tests
{
    [AllureSuite("SauceDemo Tests")] // Example Allure Suite attribute
    public class LoginTests
    {
        [Fact]
        [AllureFeature("Login")]
        [AllureStory("Successful Login")]
        [AllureOwner("YourName")] // Replace with your name or identifier
        [AllureTag("Smoke")]
        public async Task SuccessfulLoginTest()
        {
            // Initialize Playwright
            using var playwright = await Playwright.CreateAsync();

            // Launch browser
            // Set Headless = false for local debugging if you want to see the browser
            await using var browser = await playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions { Headless = true }); 
            var page = await browser.NewPageAsync();

            // Navigate to login page
            await page.GotoAsync("https://www.saucedemo.com/");

            // Perform login
            var loginPage = new LoginPage(page);
            await loginPage.Login("standard_user", "secret_sauce");

            // Verify login success
            var inventoryPage = new InventoryPage(page);
            Assert.True(await inventoryPage.IsProductsTitleVisible(), "Products title was not visible after login.");
            Assert.Equal("PRODUCTS", (await inventoryPage.GetProductsTitleText()).ToUpper()); // Site shows "PRODUCTS" in uppercase
            
            // Optional: Add more assertions if needed
            // Assert.True(await inventoryPage.IsShoppingCartVisible(), "Shopping cart was not visible after login.");
        }
    }
}
