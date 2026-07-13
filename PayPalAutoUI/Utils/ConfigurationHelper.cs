using Microsoft.Extensions.Configuration;

namespace PayPalAutoUI.Utils
{
    public static class ConfigurationHelper
    {
        private static IConfigurationRoot? _config;

        private static IConfigurationRoot Config
        {
            get
            {
                if (_config != null) return _config;

                var builder = new ConfigurationBuilder()
                    .SetBasePath(AppContext.BaseDirectory)
                    .AddJsonFile("appsettings.json", optional: false, reloadOnChange: true)
                    .AddEnvironmentVariables();

                _config = builder.Build();
                return _config;
            }
        }

        public static string GetString(string key) => Config[key] ?? string.Empty;

        public static string GetRequiredString(string key)
        {
            var value = GetString(key);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new InvalidOperationException($"Missing required configuration value: '{key}'.");
            }

            return value;
        }

        public static int GetInt(string key, int fallback = 0)
            => int.TryParse(Config[key], out var value) ? value : fallback;

        public static bool GetBool(string key, bool fallback = false)
            => bool.TryParse(Config[key], out var value) ? value : fallback;

        public static (string Email, string Password) GetAccount(string accountName)
        {
            var email = Config[$"Accounts:{accountName}:Email"] ?? string.Empty;
            var password = Config[$"Accounts:{accountName}:Password"] ?? string.Empty;

            return (email, password);
        }

        public static (string Email, string Password) GetRequiredAccount(string accountName)
        {
            var email = GetRequiredString($"Accounts:{accountName}:Email");
            var password = GetRequiredString($"Accounts:{accountName}:Password");

            return (email, password);
        }

        public static string SignInUrl => GetString("BaseUrls:SignIn");
        public static int DefaultTimeoutMs => GetInt("Timeouts:DefaultMs", 10000);
        public static bool HeadlessBrowser => GetBool("Browser:Headless", true);
        public static int SlowMoMs => GetInt("Browser:SlowMoMs", 0);
    }
}
