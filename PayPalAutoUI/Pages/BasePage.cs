using Microsoft.Playwright;
using PayPalAutoUI.Utils;

namespace PayPalAutoUI.Pages
{
    public abstract class BasePage
    {
        protected readonly IPage Page;
        protected static int DefaultTimeoutMs => ConfigurationHelper.DefaultTimeoutMs;

        protected BasePage(IPage page) => Page = page;

        protected Task ClickAsync(string selector) => Page.ClickAsync(selector);
        protected Task TypeAsync(string selector, string text) => Page.FillAsync(selector, text);

        protected async Task<bool> IsVisibleAsync(string selector, int timeoutMs = 5000)
        {
            try
            {
                await Page.Locator(selector).First.WaitForAsync(new()
                {
                    State = WaitForSelectorState.Visible,
                    Timeout = timeoutMs
                });

                return true;
            }
            catch
            {
                return false;
            }
        }

        protected Task WaitForSelectorAsync(string selector, int? timeoutMs = null)
            => Page.Locator(selector).First.WaitForAsync(new()
            {
                State = WaitForSelectorState.Attached,
                Timeout = timeoutMs ?? DefaultTimeoutMs
            });

        protected async Task DismissCookieBannerAsync()
        {
            var acceptButton = Page.Locator("button:has-text('Yes, I accept')");

            if (await acceptButton.CountAsync() > 0 && await acceptButton.First.IsVisibleAsync())
            {
                await acceptButton.First.ClickAsync();
            }
        }

        protected async Task BlurActiveElementAsync()
        {
            var safe = Page.Locator("main, header, body").First;

            if (await safe.CountAsync() > 0)
            {
                await safe.ClickAsync();
            }
            else
            {
                await Page.Keyboard.PressAsync("Tab");
            }
        }
    }
}
