using Microsoft.Playwright;
using PayPalAutoUI.Utils;

namespace PayPalAutoUI.Drivers
{
    public static class PlaywrightDriver
    {
        public static IPlaywright Playwright { get; private set; } = null!;
        public static IBrowser Browser { get; private set; } = null!;
        public static IBrowserContext Context { get; private set; } = null!;
        public static IPage Page { get; private set; } = null!;

        public static async Task InitializeAsync()
        {
            Playwright = await Microsoft.Playwright.Playwright.CreateAsync();
            Browser = await Playwright.Chromium.LaunchAsync(new BrowserTypeLaunchOptions
            {
                Headless = ConfigurationHelper.HeadlessBrowser,
                SlowMo = ConfigurationHelper.SlowMoMs
            });

            Context = await Browser.NewContextAsync(new BrowserNewContextOptions
            {
                ViewportSize = new ViewportSize
                {
                    Width = 1440,
                    Height = 900
                }
            });

            Context.SetDefaultTimeout(ConfigurationHelper.DefaultTimeoutMs);
            Page = await Context.NewPageAsync();
        }

        public static async Task CleanupAsync()
        {
            if (Page is not null)
            {
                await Page.CloseAsync();
            }

            if (Context is not null)
            {
                await Context.CloseAsync();
            }

            if (Browser is not null)
            {
                await Browser.CloseAsync();
            }

            Playwright?.Dispose();
        }
    }
}
