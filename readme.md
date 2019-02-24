# WaaS [![Build status](https://dev.azure.com/WebE-WaaS/WaaS/_apis/build/status/WebE-WaaS%20-%20CI)](https://dev.azure.com/WebE-WaaS/WaaS/_build/latest?definitionId=1)

<!-- markdownlint-disable MD026 -->

## What's WaaS?

<!-- markdownlint-enable MD026 -->

WaaS is short for Web scraper as a service. It's a web-application that allows users to define crawls that look for a RegEx Pattern on an URL. WaaS will periodically check the website for matching patterns and notify the users via E-Mail.

## How to run it

### Prerequisites

To run WaaS locally you need the following software:

* Node.js <https://nodejs.org>
* .NET Core SDK <https://dotnet.microsoft.com/download>

We're using Visual Studio to run and debug the back-end of the app but you can also run the app with the `dotnet` cli tool included in the .NET Core SDK by running `dotnet run WaaS.Presentation.csproj` in the `WaaS.Presentation` folder.

1. Clone or download this git repository.
2. Open a cli in the `WaaS\WaaS.Presentation\ClientApp` folder.
3. Download the required npm dependencies using `npm i`
4. Start the Angular SPA using `npm start`.
5. Open the WaaS.sln in Visual Studio.
6. Set WaaS.Presentation as the Startup project.
7. Start the application.
