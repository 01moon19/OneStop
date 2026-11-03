# Stage 1: Build
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ECommerce.ProductService/ECommerce.ProductService.csproj ECommerce.ProductService/

RUN dotnet restore ECommerce.ProductService/ECommerce.ProductService.csproj

COPY ECommerce.ProductService/ ECommerce.ProductService/

WORKDIR /src/ECommerce.ProductService

RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.ProductService.dll"]