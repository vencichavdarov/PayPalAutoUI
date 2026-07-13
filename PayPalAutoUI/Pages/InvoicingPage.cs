using Microsoft.Playwright;
namespace PayPalAutoUI.Pages
{
    public class InvoicingPage : BasePage
    {
        private readonly string _invoicingMenuLink = "a[id='quicklinks_appcard2']";
        private readonly string _createNewButton = "button[id='action-btn-2']";
        private readonly string _createNewMenuItems = "nav[data-ppui-info*='action-menu'] menu[role='menu'] li[role='menuitem']";
        private readonly string _customerInput = "input[id='combo_txt_recipient-input-combobox']";
        private readonly string _createNewCustomerOption = "div[data-value='ADD_NEW_CUSTOMER']";
        private readonly string _nameInputCustomer = "input[id='text-input-name']";
        private readonly string _emailInputCustomer = "input[id='text-input-email']";
        private readonly string _addCustomerButton = "button[data-testid='quick-add-button']";
        private readonly string _itemNameInput = "input[placeholder='Name'], input[data-testid='item-name-input']";
        private readonly string _itemQuantityInput = "input[placeholder='Qty'], input[id='text-input-quantity']";
        private readonly string _itemPriceInput = "input[placeholder='Price per unit'], input[id='text-input-unit_price']";
        private readonly string _itemAddButton = "button[data-testid='save-item-btn']";
        private readonly string _noteTextArea = "textarea[id='text-input-note']";
        private readonly string _sendButton = "button:has-text('Send')";
        private readonly string _successMessage = "div h3";
        private readonly string _warningMessages = "div[role='alert']";

        public InvoicingPage(IPage page) : base(page) { }

        public async Task NavigateToInvoicingAsync()
        {
            await ClickAsync(_invoicingMenuLink);
            await DismissCookieBannerAsync();
            await WaitForSelectorAsync(_createNewButton);
        }

        public async Task OpenCreateNewMenuAsync()
        {
            await ClickAsync(_createNewButton);
            await Page.Locator(_createNewMenuItems).First.WaitForAsync(new()
            {
                State = WaitForSelectorState.Attached,
                Timeout = DefaultTimeoutMs
            });
        }

        public async Task SelectCreateNewInvoiceAsync()
        {
            await OpenCreateNewMenuAsync();

            var invoiceItem = Page.Locator(_createNewMenuItems).First;
            await invoiceItem.ClickAsync();

            await WaitForSelectorAsync(_customerInput);
        }

        public async Task AddNewCustomerAsync(string name, string email)
        {
            await ClickAsync(_customerInput);
            await ClickAsync(_createNewCustomerOption);

            await TypeAsync(_nameInputCustomer, name);
            await TypeAsync(_emailInputCustomer, email);

            await ClickAsync(_addCustomerButton);
        }

        public async Task AddNewItemAsync(string itemName, string quantity, string unitPrice)
        {
            await TypeAsync(_itemNameInput, itemName);
            await TypeAsync(_itemQuantityInput, quantity);
            await TypeAsync(_itemPriceInput, unitPrice);

            await ClickAsync(_itemAddButton);
        }

        public async Task AddNoteAsync(string note)
        {
            await TypeAsync(_noteTextArea, note);
        }

        public async Task SendInvoiceAsync()
        {
            await DismissCookieBannerAsync();
            await ClickAsync(_sendButton);
        }

        public Task<bool> IsInvoiceSentAsync() => IsVisibleAsync(_successMessage);

        public async Task<List<string>> GetAllWarningMessagesAsync(int timeoutMs = 10000)
        {
            var warnings = Page.Locator(_warningMessages);

            await warnings.First.WaitForAsync(new()
            {
                State = WaitForSelectorState.Visible,
                Timeout = timeoutMs
            });

            var count = await warnings.CountAsync();
            var messages = new List<string>(count);

            for (var i = 0; i < count; i++)
            {
                var text = await warnings.Nth(i).InnerTextAsync();
                messages.Add(text);
            }

            return messages;
        }
    }
}

