# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ECommerce.OrderService/ECommerce.OrderService.csproj ECommerce.OrderService/

RUN dotnet restore ECommerce.OrderService/ECommerce.OrderService.csproj

COPY ECommerce.OrderService/ ECommerce.OrderService/

WORKDIR /src/ECommerce.OrderService

RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.OrderService.dll"]