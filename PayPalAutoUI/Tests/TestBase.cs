using NUnit.Framework.Interfaces;
using PayPalAutoUI.Drivers;
using PayPalAutoUI.Pages;
using PayPalAutoUI.Utils;

namespace PayPalAutoUI.Tests
{
    public abstract class TestBase
    {
        [SetUp]
        public virtual async Task Setup()
        {
            await PlaywrightDriver.InitializeAsync();
        }

        [TearDown]
        public virtual async Task Teardown()
        {
            if (TestContext.CurrentContext.Result.Outcome.Status == TestStatus.Failed && PlaywrightDriver.Page is not null)
            {
                Directory.CreateDirectory("TestResults");

                var screenshotPath = Path.Combine(
                    "TestResults",
                    $"{TestContext.CurrentContext.Test.Name}-{DateTime.UtcNow:yyyyMMddHHmmss}.png");

                await PlaywrightDriver.Page.ScreenshotAsync(new() { Path = screenshotPath, FullPage = true });
                TestContext.AddTestAttachment(screenshotPath);
            }

            await PlaywrightDriver.CleanupAsync();
        }

        protected static (string Email, string Password) BusinessAccount
            => ConfigurationHelper.GetRequiredAccount("Business");

        protected static string PersonalEmail
            =>  ConfigurationHelper.GetRequiredString("Accounts:Personal:Email");

        protected static string RecipientEmail
            => ConfigurationHelper.GetRequiredString("Accounts:Recipient:Email");

        protected async Task LoginAsBusinessUserAsync()
        {
            var loginPage = new LoginPage(PlaywrightDriver.Page);
            var businessAccount = BusinessAccount;

            await loginPage.NavigateAsync();
            await loginPage.LoginAsync(businessAccount.Email, businessAccount.Password);
        }
    }
}
