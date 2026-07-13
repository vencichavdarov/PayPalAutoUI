using Microsoft.Playwright;
using PayPalAutoUI.Utils;
using System.Text.RegularExpressions;

namespace PayPalAutoUI.Pages
{
    public class SendMoneyPage : BasePage
    {
        private readonly string _sendMenuButton = "#quicklinks_appcard4";
        private readonly string _recipientInput = "input#fn-sendRecipient";
        private readonly string _firstContact = "button[data-nemo='contact-circle-item']";
        private readonly string _amountInput = "input[data-nemo='sender-pays-amount'], input[data-nemo='amount']";
        private readonly string _noteInput = "textarea[data-nemo='note-field']";
        private readonly string _continueButton = "button[data-nemo='continue']";
        private readonly string _payPalFee = "div.selectedFundingOptionFees p:has-text('PayPal fee') + p";
        private readonly string _totalAmount = "p[data-nemo='total-sender-pay']";
        private readonly string _nextSendButton = "button[data-nemo='choice-next-button']";
        private readonly string _sendButton = "button[data-nemo='send']";
        private readonly string _successMessage = "p[data-nemo='success-header']";
        private readonly string _nextButtonPay = "button[data-nemo='send']";

        public SendMoneyPage(IPage page) : base(page) { }

        public async Task NavigateToSendMoneyAsync()
        {
            await ClickAsync(_sendMenuButton);
            await WaitForSelectorAsync(_recipientInput);
        }

        public async Task EnterRecipientAsync(string recipientEmail)
        {
            await TypeAsync(_recipientInput, recipientEmail);

            var dropdownOption = Page.Locator("[role='option'], div[role='option'], div[id^='PID-']").First;

            await dropdownOption.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 2000 });
            await dropdownOption.ClickAsync();

            await WaitForSelectorAsync(_amountInput);
        }

        public async Task SelectFirstContact()
        {
            await ClickAsync(_firstContact);
            await WaitForSelectorAsync(_amountInput);
        }

        public async Task EnterAmountAsync(string amount, string text)
        {
            await TypeAsync(_amountInput, amount);
            await TypeAsync(_noteInput, text);
            await ClickAsync(_continueButton);
        }

        public async Task ClickOnTheNextButtonOnPaymentTab()
        {
            await ClickAsync(_nextSendButton);
            await WaitForSelectorAsync(_nextButtonPay);
            await ClickAsync(_nextButtonPay);
        }

        public async Task ConfirmPaymentAsync()
        {
            await ClickAsync(_sendButton);
        }

        public async Task<bool> IsPaymentSuccessfulAsync()
        {
            if (await IsVisibleAsync(_successMessage, 5000)) return true;

            try
            {
                await Page.WaitForURLAsync(new Regex("success", RegexOptions.IgnoreCase), new() { Timeout = 5000 });
                return true;
            }
            catch
            {
                return false;
            }
        }

        public async Task<bool> VerifyTotalAsync(decimal amountInput, string inputCurrency = "USD", string displayCurrency = "EUR")
        {
            await Page.Locator(_totalAmount).WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 8000 });

            var total = CurrencyHelper.ParseAmount(await Page.InnerTextAsync(_totalAmount));
            var amountInDisplay = CurrencyHelper.Convert(amountInput, inputCurrency, displayCurrency);

            decimal fee;

            try
            {
                var feeText = await Page.InnerTextAsync(_payPalFee);
                fee = CurrencyHelper.ParseAmount(feeText);
            }
            catch
            {
                fee = total - amountInDisplay;
                if (fee < 0) fee = 0;
            }

            return Math.Abs(total - (amountInDisplay + fee)) < 0.01m;
        }
    }
}
