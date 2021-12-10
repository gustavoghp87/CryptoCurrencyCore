
# ENV ASPNETCORE_URLS=http://+:80
# ENV DOTNET_USE_POLLING_FILE_WATCHER=true
# ENV ASPNETCORE_ENVIRONMENT=Development
# EXPOSE 80
# ENTRYPOINT ["dotnet", "CryptoCurrency.dll"]
##################################################################################

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
COPY --from=publish /app/out .

#ENV domainName=https://mysterious-thicket-34741.herokuapp.com/
ENV domainName=https://limitless-sands-00250.herokuapp.com/
CMD ASPNETCORE_URLS=http://*:$PORT dotnet CryptoCurrency.dll

#ENV domainName=http://190.231.194.136/
#ENV domainName=http://190.231.194.136:8081/
#CMD ASPNETCORE_URLS=http://*:8080 dotnet CryptoCurrency.dll
