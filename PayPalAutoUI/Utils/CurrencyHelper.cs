using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PayPalAutoUI.Utils
{
    public static class CurrencyHelper
    {
        public const decimal UsdToEurRate = 0.8351m;

        public static decimal Convert(decimal amount, string fromCurrency, string toCurrency)
        {
            var from = (fromCurrency ?? "").Trim().ToUpperInvariant();
            var to = (toCurrency ?? "").Trim().ToUpperInvariant();

            if (from == to) return amount;

            if (from == "USD" && to == "EUR")
                return Math.Round(amount * UsdToEurRate, 2);

            if (from == "EUR" && to == "USD")
                return Math.Round(amount / UsdToEurRate, 2);

            return amount;
        }
        public static decimal ParseAmount(string text)
        {
            if (string.IsNullOrWhiteSpace(text)) return 0m;

            var cleaned = text
                .Replace("\u00A0", " ")
                .Replace("EUR", "", StringComparison.OrdinalIgnoreCase)
                .Replace("USD", "", StringComparison.OrdinalIgnoreCase)
                .Replace("€", "").Replace("$", "")
                .Trim();

            // махни всички интервали
            cleaned = cleaned.Replace(" ", "");

            // ако има запетая – приемаме я като десетичен разделител
            cleaned = cleaned.Replace(',', '.');

            // остави само цифри, точка и минус
            var only = new string(cleaned.Where(c => char.IsDigit(c) || c == '.' || c == '-').ToArray());

            if (decimal.TryParse(only, NumberStyles.Number | NumberStyles.AllowLeadingSign, CultureInfo.InvariantCulture, out var val))
                return val;

            throw new FormatException($"Cannot parse amount: '{text}'");
        }
    }
}
