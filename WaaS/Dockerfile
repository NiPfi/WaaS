FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
# Get Node.js for npm scripts
COPY --from=node:lts-alpine / /
WORKDIR /app
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000
EXPOSE 5001

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY . .
RUN dotnet restore "WaaS.Presentation/WaaS.Presentation.csproj"
WORKDIR /src/WaaS.Presentation
RUN dotnet build "WaaS.Presentation.csproj" -c Release -o /app

FROM build AS publish
COPY --from=node:lts-alpine / /
WORKDIR /src/WaaS.Presentation
RUN dotnet publish "WaaS.Presentation.csproj" -c Release -o /app
WORKDIR /src/WaaS.Presentation/ClientApp

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WaaS.Presentation.dll"]
