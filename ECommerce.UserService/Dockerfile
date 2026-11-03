
# Stage 1: Build and publish
FROM mcr.microsoft.com/dotnet/sdk:10.0 AS build

WORKDIR /src

COPY ECommerce.UserService/ECommerce.UserService.csproj ECommerce.UserService/

RUN dotnet restore ECommerce.UserService/ECommerce.UserService.csproj

COPY ECommerce.UserService/ ECommerce.UserService/

WORKDIR /src/ECommerce.UserService

RUN dotnet publish -c Release -o /app/publish --no-restore

# Stage 2: Runtime
FROM mcr.microsoft.com/dotnet/aspnet:10.0

WORKDIR /app

COPY --from=build /app/publish .

ENV ASPNETCORE_HTTP_PORTS=8080

EXPOSE 8080

ENTRYPOINT ["dotnet", "ECommerce.UserService.dll"]
