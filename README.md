# PayPalAutoUI
.NET 8 UI automation project for PayPal Sandbox flows, built with NUnit and Microsoft Playwright. It demonstrates a maintainable Page Object Model structure, environment-driven test configuration, and end-to-end coverage for login, money transfer, money request, and invoicing workflows.
## Tech stack

- C# / .NET 8
- NUnit
- Microsoft Playwright
- Page Object Model
- JSON and environment-based configuration
## Covered scenarios

- Valid and invalid login validation
- Send money flow
- Request money flow
- Invoice creation and required-field warning validation
## Configuration

The committed `appsettings.json` intentionally does not contain credentials. Provide PayPal Sandbox values through environment variables before running the suite:

export Accounts__Business__Email="your-business-sandbox-email"
export Accounts__Business__Password="your-business-sandbox-password"
export Accounts__Personal__Email="recipient-personal-sandbox-email"
export Accounts__Recipient__Email="recipient-business-sandbox-email"
## Running the Tests
Install the project dependencies and Playwright browsers:

```bash
dotnet restore
dotnet build
./PPalAutoUI/bin/Debug/net8.0/playwright.sh install
```
Run the full test suite:

```bash
dotnet test
```
## Notes
These tests target PayPal Sandbox, so selectors and flows can change when the third-party UI changes. Credentials should always stay outside source control and be injected locally or through CI secrets.

