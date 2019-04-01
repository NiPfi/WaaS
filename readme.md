# WaaS 
[![Build status](https://dev.azure.com/WebE-WaaS/WaaS/_apis/build/status/WebE-WaaS%20-%20CI)](https://dev.azure.com/WebE-WaaS/WaaS/_build/latest?definitionId=1)

[![Quality Gate Status](https://sonarcloud.io/api/project_badges/measure?project=webe-waas&metric=alert_status)](https://sonarcloud.io/dashboard?id=webe-waas)
[![Maintainability Rating](https://sonarcloud.io/api/project_badges/measure?project=webe-waas&metric=sqale_rating)](https://sonarcloud.io/dashboard?id=webe-waas)
[![Reliability Rating](https://sonarcloud.io/api/project_badges/measure?project=webe-waas&metric=reliability_rating)](https://sonarcloud.io/dashboard?id=webe-waas)
[![Security Rating](https://sonarcloud.io/api/project_badges/measure?project=webe-waas&metric=security_rating)](https://sonarcloud.io/dashboard?id=webe-waas)

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

## Links
* CI/CD Pipelines on Azure DevOps: <https://dev.azure.com/WebE-WaaS/WaaS/>
* Static code analysis with SonarCloud: <https://sonarcloud.io/dashboard?id=webe-waas>
* CI Deployment Environment: <https://webe-waas.azurewebsites.net>
