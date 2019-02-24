FROM microsoft/dotnet:2.2-aspnetcore-runtime AS base
WORKDIR /app
EXPOSE 443

FROM microsoft/dotnet:2.2-sdk AS build
WORKDIR /src
COPY ["WaaS.Presentation/WaaS.Presentation.csproj", "WaaS.Presentation/"]
RUN dotnet restore "WaaS.Presentation/WaaS.Presentation.csproj"
COPY . .
WORKDIR "/src/WaaS.Presentation"
RUN dotnet build "WaaS.Presentation.csproj" -c Release -o /app

FROM build AS publish
RUN dotnet publish "WaaS.Presentation.csproj" -c Release -o /app

FROM base AS final
WORKDIR /app
COPY --from=publish /app .
ENTRYPOINT ["dotnet", "WaaS.Presentation.dll"]
