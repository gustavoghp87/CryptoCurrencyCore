#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /app
COPY *.sln .
COPY ["CryptoCurrency/CryptoCurrency.csproj", "CryptoCurrency/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Services/Services.csproj", "Services/"]
COPY ["Tests/Tests.csproj", "Tests/"]
RUN dotnet restore
COPY . .

FROM build AS publish
WORKDIR "/app"
RUN dotnet publish -c Release -o out

FROM base AS final
WORKDIR /app
# ENV ASPNETCORE_URLS=http://+:80
# ENV DOTNET_USE_POLLING_FILE_WATCHER=true
# ENV ASPNETCORE_ENVIRONMENT=Development
# EXPOSE 80
COPY --from=publish /app/out .
# ENTRYPOINT ["dotnet", "CryptoCurrency.dll"]
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CryptoCurrency.dll
