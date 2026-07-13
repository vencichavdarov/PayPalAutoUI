
using Microsoft.Playwright;
using PayPalAutoUI.Utils;

namespace PayPalAutoUI.Pages
{
    public class LoginPage : BasePage
    {
        private readonly string _emailInput = "input#email";
        private readonly string _passwordInput = "input#password";
        private readonly string _loginButton = "button#btnLogin";
        private readonly string _nextButton = "button#btnNext";
        private readonly string _validationMessage = "p[role='alert']";

        public LoginPage(IPage page) : base(page) { }

        public async Task NavigateAsync() => await Page.GotoAsync(ConfigurationHelper.SignInUrl);

        public async Task LoginAsync(string email, string password)
        {
            await WaitForSelectorAsync(_emailInput);
            await TypeAsync(_emailInput, email);

            await ClickAsync(_nextButton);

            await WaitForSelectorAsync(_passwordInput);
            await TypeAsync(_passwordInput, password);

            await ClickAsync(_loginButton);
        }

        public async Task<string> GetValidationMessageAsync()
        {
            var validationMessage = Page.Locator(_validationMessage).First;

            await validationMessage.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = 5000
            });

            return await validationMessage.InnerTextAsync();
        }
    }
}

