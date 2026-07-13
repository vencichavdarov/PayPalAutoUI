using Microsoft.Playwright;
using PayPalAutoUI.Drivers;
using PayPalAutoUI.Pages;
using System.Globalization;

namespace PayPalAutoUI.Tests
{
    public class InvoicingTests : TestBase
    {
        [Test]
        public async Task CreateAndSendInvoiceShouldShowSuccess()
        {
            await LoginAsBusinessUserAsync();

            var invoicing = new InvoicingPage(PlaywrightDriver.Page);
            await invoicing.NavigateToInvoicingAsync();
            await invoicing.SelectCreateNewInvoiceAsync();

            var unique = DateTime.UtcNow.ToString("yyyyMMddHHmmss");
            var customerName = $"Customer {unique}";
            var customerEmail = $"qa{unique}@gmail.com";
            await invoicing.AddNewCustomerAsync(customerName, customerEmail);

            var itemName = $"Item {unique}";
            await invoicing.AddNewItemAsync(itemName, "2", 15.00m.ToString("0.##", CultureInfo.InvariantCulture));

            await invoicing.AddNoteAsync("Automated Note");
            await invoicing.SendInvoiceAsync();

            Assert.That(await invoicing.IsInvoiceSentAsync(), Is.True, "Expected success message after sending the invoice.");
        }

        [Test]
        public async Task VerifyWarningMessagesForRequiredItems()
        {
            await LoginAsBusinessUserAsync();

            var invoicing = new InvoicingPage(PlaywrightDriver.Page);
            await invoicing.NavigateToInvoicingAsync();
            await invoicing.SelectCreateNewInvoiceAsync();

            await invoicing.SendInvoiceAsync();

            var expectedMessages = new[]
            {
                "To send the invoice, add the items the customer is paying for."
            };

            await Assertions.Expect(
                PlaywrightDriver.Page.Locator("div[role='alert']")
            ).ToContainTextAsync(expectedMessages, new() { Timeout = 10000 });

            var actualMessages = await invoicing.GetAllWarningMessagesAsync();

            foreach (var expected in expectedMessages)
            {
                Assert.That(
                    actualMessages,
                    Has.Some.EqualTo(expected),
                    $"Expected warning '{expected}' was not found. Actual warnings: {string.Join(" | ", actualMessages)}"
                );
            }
        }
    }
}
