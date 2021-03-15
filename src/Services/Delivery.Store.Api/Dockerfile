#See https://aka.ms/containerfastmode to understand how Visual Studio uses this Dockerfile to build your images for faster debugging.

FROM mcr.microsoft.com/dotnet/aspnet:5.0 AS base
WORKDIR /app
EXPOSE 80
EXPOSE 443

FROM mcr.microsoft.com/dotnet/sdk:5.0 AS build
WORKDIR /src
#COPY ["src/Services/Delivery.Store.Api/Delivery.Store.Api.csproj", "src/Services/Delivery.Store.Api/"]
COPY . ./
WORKDIR /tests
#COPY ["src/Services/Delivery.Store.Api/Delivery.Store.Api.csproj", "src/Services/Delivery.Store.Api/"]
COPY . ./
WORKDIR /src
RUN dotnet restore "src/Services/Delivery.Store.Api/Delivery.Store.Api.csproj"
COPY . .
WORKDIR "/src/src/Services/Delivery.Store.Api"

FROM build AS tests
WORKDIR /tests
RUN dotnet restore "tests/Delivery.Store.Api.Tests/Delivery.Store.Api.Tests.csproj"
COPY . .
WORKDIR "tests/Delivery.Store.Api.Tests"
ENTRYPOINT ["dotnet", "test", "--logger:trx"]

FROM build AS publish
WORKDIR /src
WORKDIR "/src/src/Services/Delivery.Store.Api"
RUN dotnet publish "Delivery.Store.Api.csproj" -c Release -o /app/publish

FROM base AS final
WORKDIR /app
COPY --from=publish /app/publish .
ENTRYPOINT ["dotnet", "Delivery.Store.Api.dll"]