using Microsoft.Playwright;
namespace PayPalAutoUI.Pages
{
    public class RequestMoneyPage : BasePage
    {
        private readonly string _requestMenuButton = "a[id='quicklinks_appcard3']";
        private readonly string _recipientInput = "input[data-nemo='recipient']";
        private readonly string _nextButton = "button[data-nemo='multi-request-button']";
        private readonly string _firstContact = "button[data-nemo='contact-circle-item']";
        private readonly string _amountInput = "input[data-nemo='amount']";
        private readonly string _noteInput = "textarea[data-nemo='note-field']";
        private readonly string _requestButton = "button[id='button_:r4:']";
        private readonly string _successHeader = "div[class='requestSuccess']";

        public RequestMoneyPage(IPage page) : base(page) { }

        public async Task NavigateToRequestMoneyAsync()
        {
            await ClickAsync(_requestMenuButton);
            await WaitForSelectorAsync(_recipientInput);
        }

        public async Task EnterRecipientAsync(string recipientEmail)
        {
            await TypeAsync(_recipientInput, recipientEmail);
            await BlurActiveElementAsync();
            await ClickAsync(_nextButton);

            try
            {
                await Page.Locator(_firstContact).First.WaitForAsync(new() { State = WaitForSelectorState.Visible, Timeout = 4000 });
            }
            catch
            {
                await WaitForSelectorAsync(_amountInput);
            }
        }

        public async Task SelectFirstContactAsync()
        {
            await ClickAsync(_firstContact);
            await ClickAsync(_nextButton);
            await WaitForSelectorAsync(_amountInput);
        }

        public async Task EnterAmountAndNoteAsync(string amount, string note)
        {
            await TypeAsync(_amountInput, amount);
            await TypeAsync(_noteInput, note);
        }

        public async Task ClickOnTheRequestButtonAsync()
        {
            await ClickAsync(_requestButton);
        }

        public Task<bool> IsRequestSuccessfulAsync() => IsVisibleAsync(_successHeader, 15000);
    }
}
