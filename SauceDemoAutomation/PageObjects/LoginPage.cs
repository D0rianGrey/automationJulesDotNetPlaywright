using Microsoft.Playwright;
using System.Threading.Tasks;

namespace SauceDemoAutomation.PageObjects
{
    public class LoginPage
    {
        private readonly IPage _page;
        private readonly ILocator _usernameInput;
        private readonly ILocator _passwordInput;
        private readonly ILocator _loginButton;
        private readonly ILocator _errorMessage; 

        public LoginPage(IPage page)
        {
            _page = page;
            _usernameInput = _page.Locator("#user-name");
            _passwordInput = _page.Locator("#password");
            _loginButton = _page.Locator("#login-button");
            _errorMessage = _page.Locator("h3[data-test='error']");
        }

        public async Task EnterUsername(string username)
        {
            await _usernameInput.FillAsync(username);
        }

        public async Task EnterPassword(string password)
        {
            await _passwordInput.FillAsync(password);
        }

        public async Task ClickLoginButton()
        {
            await _loginButton.ClickAsync();
        }

        public async Task Login(string username, string password)
        {
            await EnterUsername(username);
            await EnterPassword(password);
            await ClickLoginButton();
        }

        public async Task<string> GetErrorMessage()
        {
            return await _errorMessage.InnerTextAsync();
        }
    }
}
