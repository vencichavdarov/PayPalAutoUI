using PayPalAutoUI.Drivers;
using PayPalAutoUI.Pages;
using System.Globalization;

namespace PayPalAutoUI.Tests
{
    public class RequestMoneyTests : TestBase
    {
        [Test]
        public async Task RequestMoneyShouldCompleteSuccessfully()
        {
            await LoginAsBusinessUserAsync();

            var requestPage = new RequestMoneyPage(PlaywrightDriver.Page);
            await requestPage.NavigateToRequestMoneyAsync();
            await requestPage.EnterRecipientAsync(RecipientEmail);

            var amount = 7.50m;
            var amountText = amount.ToString("0.##", CultureInfo.InvariantCulture);
            await requestPage.EnterAmountAndNoteAsync(amountText, "Test request!");

            await requestPage.ClickOnTheRequestButtonAsync();

            Assert.That(await requestPage.IsRequestSuccessfulAsync(), Is.True, "Request was not successful");
        }
    }
}
