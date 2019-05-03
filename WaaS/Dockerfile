FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
# Get Node.js for npm scripts
COPY --from=node:lts-alpine / /
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000
EXPOSE 5001

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["WaaS.Presentation/WaaS.Presentation.csproj", "WaaS.Presentation/"]
COPY ["WaaS.Business/WaaS.Business.csproj", "WaaS.Business/"]
COPY ["WaaS.Infrastructure/WaaS.Infrastructure.csproj", "WaaS.Infrastructure/"]
COPY ["WaaS.Infrastructure.SendGridMail/WaaS.Infrastructure.SendGridMail.csproj", "WaaS.Infrastructure.SendGridMail/"]
RUN dotnet restore "WaaS.Presentation/WaaS.Presentation.csproj"
COPY . .
WORKDIR /src/WaaS.Presentation
RUN dotnet build "WaaS.Presentation.csproj" -c Release -o /app

FROM build AS publish
COPY --from=node:alpine / /
WORKDIR /src/WaaS.Presentation
RUN dotnet publish "WaaS.Presentation.csproj" -c Release -o /app
WORKDIR /src/WaaS.Presentation/ClientApp

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WaaS.Presentation.dll"]
