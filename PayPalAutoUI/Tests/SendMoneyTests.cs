using PayPalAutoUI.Drivers;
using PayPalAutoUI.Pages;
using System.Globalization;

namespace PayPalAutoUI.Tests
{
    public class SendMoneyTests : TestBase
    {
        [Test]
        public async Task SendMoneyShouldCalculateTotalAndCompletePayment()
        {
            await LoginAsBusinessUserAsync();

            var sendMoneyPage = new SendMoneyPage(PlaywrightDriver.Page);
            await sendMoneyPage.NavigateToSendMoneyAsync();
            await sendMoneyPage.EnterRecipientAsync(PersonalEmail);

            var amountToSend = 1000.00m;
            var amountAsText = amountToSend.ToString("0.##", CultureInfo.InvariantCulture);
            await sendMoneyPage.EnterAmountAsync(amountAsText, "Test Payment");

            await sendMoneyPage.ClickOnTheNextButtonOnPaymentTab();

            Assert.That(await sendMoneyPage.IsPaymentSuccessfulAsync(), Is.True, "Payment was not successful");
        }
    }
}
