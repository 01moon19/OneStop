
# Build stage
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build
WORKDIR /src

COPY ECommerce.APIGateway/ECommerce.APIGateway.csproj ECommerce.APIGateway/
RUN dotnet restore ECommerce.APIGateway/ECommerce.APIGateway.csproj

COPY ECommerce.APIGateway/ ECommerce.APIGateway/
WORKDIR /src/ECommerce.APIGateway

RUN dotnet publish -c Release -o /app/publish --no-restore

# Runtime stage
FROM mcr.microsoft.com/dotnet/aspnet:10.0
WORKDIR /app

ENV ASPNETCORE_HTTP_PORTS=8080
EXPOSE 8080

COPY --from=build /app/publish .

ENTRYPOINT ["dotnet", "ECommerce.APIGateway.dll"]
