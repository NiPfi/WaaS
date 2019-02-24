FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
# Get Node.js for npm scripts
COPY --from=node:alpine / /
WORKDIR /app
ENV ASPNETCORE_URLS=http://+:5000
ENV ASPNETCORE_ENVIRONMENT=Production
EXPOSE 5000

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["WaaS.Presentation/WaaS.Presentation.csproj", "WaaS.Presentation/"]
RUN dotnet restore "WaaS.Presentation/WaaS.Presentation.csproj"
COPY . .
WORKDIR /src/WaaS.Presentation
RUN dotnet build "WaaS.Presentation.csproj" -c Release -o /app

FROM build AS publish
COPY --from=node:alpine / /
WORKDIR /src/WaaS.Presentation
RUN dotnet publish "WaaS.Presentation.csproj" -c Release -o /app
WORKDIR /src/WaaS.Presentation/ClientApp
RUN npm run build:ssr
RUN ls -al
RUN cp -R dist-server /app/ClientApp/
RUN cp -R node_modules /app/ClientApp/

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WaaS.Presentation.dll"]