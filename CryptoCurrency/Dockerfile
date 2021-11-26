#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 5000
EXPOSE 5001

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
COPY ["CryptoCurrency/CryptoCurrency.csproj", "CryptoCurrency/"]
COPY ["Models/Models.csproj", "Models/"]
COPY ["Services/Services.csproj", "Services/"]
RUN dotnet restore "CryptoCurrency/CryptoCurrency.csproj"
COPY . .
WORKDIR "/src/CryptoCurrency"
RUN dotnet build "CryptoCurrency.csproj" -c Release -o /app/build

FROM build AS publish
RUN dotnet publish "CryptoCurrency.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "CryptoCurrency.dll"]