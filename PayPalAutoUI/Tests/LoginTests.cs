using Microsoft.VisualStudio.TestPlatform.ObjectModel;
using PayPalAutoUI.Pages;
using System;
using PayPalAutoUI.Drivers;
using System.Text.RegularExpressions;

namespace PayPalAutoUI.Tests
{
    public class LoginTests : TestBase
    {
        [Test]
        public async Task ValidLoginShouldNavigateToDashboard()
        {
            var loginPage = new LoginPage(PlaywrightDriver.Page);
            var businessAccount = BusinessAccount;

            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(businessAccount.Email, businessAccount.Password);

            await PlaywrightDriver.Page.WaitForURLAsync(new Regex("dashboard", RegexOptions.IgnoreCase),
                new() { Timeout = 20000 });

            Assert.That(PlaywrightDriver.Page.Url, Does.Contain("dashboard"));
        }

        [Test]
        public async Task InvalidPasswordDataShouldBeNotLogged()
        {
            var loginPage = new LoginPage(PlaywrightDriver.Page);
            var businessAccount = BusinessAccount;

            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(businessAccount.Email, "123");

            var message = await loginPage.GetValidationMessageAsync();
            Assert.That(message, Is.EqualTo("Some of your info isn't correct. Please try again."));
        }
    }
}
